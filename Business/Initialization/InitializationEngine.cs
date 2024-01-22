using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EPiServer.CodeAnalysis.Pubternality;
using EPiServer.Framework.Initialization.Internal;
using EPiServer.Framework.TypeScanner;
using EPiServer.Framework.TypeScanner.Internal;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using EPiServer.Sorting.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace EPiServer.Framework.Initialization;

public class InitializationEngine : IInitializationEngine
{
    private IList<ModuleNode> _dependencySortedList;

    private readonly HashSet<IInitializableModule> _isInitialized = new HashSet<IInitializableModule>();

    private InitializationState _initializationState;

    private ITypeScannerLookup _typeScannerLookup;

    private readonly TimeMeters _timeMeters;

    private Logging.ILogger _log = LogManager.GetLogger(typeof(InitializationEngine));

    private ServiceProviderHelper _locate;

    private ServiceConfigurationContext _configurationContext;

    private readonly IServiceCollection _services;

    internal IAssemblyScanner AssemblyScanner { get; set; }

    public IEnumerable<Assembly> Assemblies { get; set; }

    public virtual IEnumerable<IInitializableModule> Modules { get; set; }

    public ServiceProviderHelper Locate
    {
        get
        {
            return _locate ?? new ServiceProviderHelper(GetServiceLocator());
        }
        set
        {
            _locate = value;
        }
    }

    public InitializationState InitializationState
    {
        get
        {
            return _initializationState;
        }
        protected set
        {
            if (value != _initializationState)
            {
                _log.Debug("InitializationState: transition from '{0}' to '{1}'.", _initializationState, value);
                _initializationState = value;
            }
        }
    }

    public HostType HostType { get; private set; }

    private event EventHandler _initComplete;

    public event EventHandler InitComplete
    {
        add
        {
            _initComplete += value;
        }
        remove
        {
            _initComplete -= value;
        }
    }

    public InitializationEngine()
        : this((IEnumerable<IInitializableModule>)null, HostType.Undefined)
    {
    }

    public InitializationEngine(IEnumerable<IInitializableModule> modules, HostType hostType)
        : this(modules, hostType, null)
    {
    }

    public InitializationEngine(IEnumerable<IInitializableModule> modules, HostType hostType, IEnumerable<Assembly> assemblies)
        : this(null, modules, hostType, assemblies, null)
    {
    }

    public InitializationEngine(IServiceCollection services)
        : this(services, HostType.WebApplication)
    {
    }

    public InitializationEngine(IServiceCollection services, HostType hostType)
        : this(services, hostType, null)
    {
    }

    public InitializationEngine(IServiceCollection services, HostType hostType, IEnumerable<Assembly> assemblies)
        : this(services, null, hostType, assemblies, null)
    {
    }

    [Internal]
    public InitializationEngine(IServiceCollection services, IEnumerable<IInitializableModule> modules, HostType hostType, IEnumerable<Assembly> assemblies, IAssemblyScanner assemblyScanner)
    {
        _services = services;
        Modules = modules;
        HostType = hostType;
        Assemblies = assemblies ?? (from a in AppDomain.CurrentDomain.GetAssemblies()
                                    where a.IsScanAllowed()
                                    select a).ToList();
        AssemblyScanner = assemblyScanner ?? new ReflectionAssemblyScanner();
        TimeMeters.Clear();
        _timeMeters = TimeMeters.Register(typeof(InitializationEngine));
    }

    private static IServiceProvider GetServiceLocator()
    {
        IServiceProvider current = ServiceLocator.Current;
        if (current != null)
        {
            return current;
        }
        throw new InvalidOperationException("ServiceLocator.Current has not been initialized.");
    }

    private static int ByDependencyCountThenByFullName(ModuleNode firstNode, ModuleNode secondNode)
    {
        int num = firstNode.DependencyCount - secondNode.DependencyCount;
        if (num == 0)
        {
            return string.Compare(firstNode.ModuleType.FullName, secondNode.ModuleType.FullName, StringComparison.Ordinal);
        }
        return num;
    }

    private static List<ModuleNode> UnresolvedListFromModules(IEnumerable<IInitializableModule> modules)
    {
        List<ModuleNode> list = new List<ModuleNode>(100);
        list.AddRange(modules.Select((IInitializableModule m) => new ModuleNode(m)));
        list.Sort(ByDependencyCountThenByFullName);
        return list;
    }

    private static IList<ModuleNode> CreateDependencySortedModules(IEnumerable<IInitializableModule> modules)
    {
        return CreateDependencySortedModules(null, modules);
    }

    private static IList<ModuleNode> CreateDependencySortedModules(IList<ModuleNode> existingList, IEnumerable<IInitializableModule> modules)
    {
        List<ModuleNode> unresolved = null;
        if (existingList != null)
        {
            List<IInitializableModule> source = modules.Where((IInitializableModule m) => !existingList.Any((ModuleNode e) => e.Module.GetType() == m.GetType())).ToList();
            if (!source.Any())
            {
                return existingList;
            }
            unresolved = new List<ModuleNode>(existingList);
            unresolved.AddRange(source.Select((IInitializableModule t) => new ModuleNode(t)));
            unresolved.Sort(ByDependencyCountThenByFullName);
        }
        else
        {
            unresolved = UnresolvedListFromModules(modules);
        }
        try
        {
            return NamedDependencySorter.Sort(unresolved).ToList();
        }
        catch (NamedDependencySortException ex)
        {
            throw new InitializationException(ex.Message, null, (from d in ex.MissingDependencies.Concat(ex.CircularDependencies)
                                                                 select unresolved.FirstOrDefault((ModuleNode m) => ((INamedDependency)m).Name.Equals(d.Name))?.ModuleType).ToArray());
        }
    }

    public IList<IInitializableModule> GetDependencySortedModules()
    {
        return (from mn in CreateDependencySortedModules(Modules)
                select mn.Module).ToList();
    }

    public void ScanAssemblies()
    {
        SetupAssemblyTypeScanner();
        if (_typeScannerLookup == null)
        {
            return;
        }
        Modules = (from moduleType in _typeScannerLookup.AllTypes.Where((Type t) => t.GetCustomAttributes(inherit: false).Any((object attr) => attr.GetType() == typeof(InitializableModuleAttribute) || attr.GetType() == typeof(ModuleDependencyAttribute))).Distinct()
                   select Activator.CreateInstance(moduleType, null)).Cast<IInitializableModule>().ToList();
    }

    public virtual ITypeScannerLookup BuildTypeScanner()
    {
        if (Assemblies == null)
        {
            throw new InvalidOperationException("Assemblies have not been added for type scanning");
        }
        SetupAssemblyTypeScanner();
        return _typeScannerLookup;
    }

    private void SetupAssemblyTypeScanner()
    {
        if (Assemblies != null)
        {
            AssemblyScanner.Configure(Assemblies);
            InitializationEngineTypeScanner initializationEngineTypeScanner = new InitializationEngineTypeScanner(AssemblyScanner);
            _typeScannerLookup = initializationEngineTypeScanner.CreateLookup(Assemblies);
            Task.Factory.StartNew(delegate
            {
                AssemblyScanner.Save();
            });
        }
    }

    public void Initialize()
    {
        _log = LogManager.GetLogger(typeof(InitializationEngine));
        _log.Information("Initialization started");
        if (Modules == null)
        {
            ScanAssemblies();
        }
        ExecuteTransition(continueTransitions: true);
        _log.Information("Initialization completed");
    }

    public void Configure()
    {
        if (InitializationState != 0)
        {
            throw new InvalidOperationException("Configure must be called on uninitialized engine");
        }
        if (Modules == null)
        {
            ScanAssemblies();
        }
        ExecuteTransition(continueTransitions: false);
    }

    public virtual void ConfigureModules(IEnumerable<IInitializableModule> modules)
    {
        if (InitializationState != 0)
        {
            throw new InvalidOperationException("ConfigureModules must be called on uninitialized engine");
        }
        if (modules == null || !modules.Any())
        {
            return;
        }
        IEnumerable<IInitializableModule> existingModules = Modules ?? Enumerable.Empty<IInitializableModule>();
        List<IInitializableModule> list = modules.Where((IInitializableModule n) => !existingModules.Any((IInitializableModule e) => n.GetType() == e.GetType())).ToList();
        if (list.Any())
        {
            IEnumerable<IInitializableModule> source;
            if (Modules == null)
            {
                IEnumerable<IInitializableModule> enumerable = list;
                source = enumerable;
            }
            else
            {
                source = Modules.Union(list);
            }
            Modules = source.ToList();
            _dependencySortedList = CreateDependencySortedModules(_dependencySortedList, list);
            ConfigureCurrentModules(final: false);
        }
    }

    private void ExecuteTransition(bool continueTransitions)
    {
        ValidateHostType();
        do
        {
            switch (InitializationState)
            {
                default:
                    return;
                case InitializationState.PreInitialize:
                    Modules = Modules.ToList();
                    _dependencySortedList = CreateDependencySortedModules(_dependencySortedList, Modules);
                    ConfigureCurrentModules(final: true);
                    InitializationState = InitializationState.Initializing;
                    continue;
                case InitializationState.InitializeFailed:
                case InitializationState.InitializeDelayed:
                case InitializationState.Uninitialized:
                    InitializationState = InitializationState.Initializing;
                    continue;
                case InitializationState.Initializing:
                    InitializeModules();
                    if (InitializationState == InitializationState.InitializeDelayed)
                    {
                        return;
                    }
                    break;
                case InitializationState.InitializeComplete:
                    break;
                case InitializationState.WaitingBeginRequest:
                case InitializationState.Initialized:
                case InitializationState.Uninitializing:
                case InitializationState.UninitializeFailed:
                    return;
            }
            OnInitComplete();
            InitializationState = InitializationState.Initialized;
        }
        while (continueTransitions);
    }

    private void ValidateHostType()
    {
        if (HostType != HostType.WebApplication && HostType != HostType.TestFramework && HostType != HostType.LegacyMirroringAppDomain)
        {
            throw new InvalidOperationException($"Initializing with unsupported host type {HostType}");
        }
    }

    private ServiceConfigurationContext GetServiceConfigurationContext()
    {
        if (_configurationContext == null)
        {
            _services.AddSingleton(GetTypeScannerLookup());
            _services.AddSingleton(this);
            _services.Forward<InitializationEngine, IInitializationEngine>();
            _configurationContext = new ServiceConfigurationContext(HostType, _services)
            {
                TypeScannerLookup = GetTypeScannerLookup()
            };
        }
        return _configurationContext;
    }

    private void ConfigureCurrentModules(bool final)
    {
        ServiceConfigurationContext serviceConfigurationContext = GetServiceConfigurationContext();
        foreach (ModuleNode item in _dependencySortedList.Where((ModuleNode m) => m.CurrentState == InitializationState.PreInitialize))
        {
            item.ConfigureContainer(serviceConfigurationContext);
        }
        if (final)
        {
            serviceConfigurationContext.RaiseConfigurationComplete();
        }
    }

    internal ITypeScannerLookup GetTypeScannerLookup()
    {
        if (_typeScannerLookup == null)
        {
            SetupAssemblyTypeScanner();
        }
        return _typeScannerLookup;
    }

    protected void InitializeModules()
    {
        foreach (ModuleNode dependencySorted in _dependencySortedList)
        {
            if (IsInitialized(dependencySorted.Module))
            {
                continue;
            }
            string text = NameForLogging(dependencySorted.Module.Initialize);
            try
            {
                dependencySorted.Initialize(this);
            }
            catch (TerminateInitializationException exception)
            {
                _log.Warning("Initialize action '" + text + "' terminated processing.", exception);
                InitializationState = InitializationState.InitializeDelayed;
                break;
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled())
                {
                    _log.Error("Initialize action failed for '" + text + "'", ex);
                }
                InitializationState = InitializationState.InitializeFailed;
            }
            if (_log.IsDebugEnabled())
            {
                _log.Debug("Initialize action successful for '{0}'", text);
            }
            _isInitialized.Add(dependencySorted.Module);
        }
    }

    public void Uninitialize()
    {
        _log.Information("Uninitialization started");
        if (_dependencySortedList == null)
        {
            throw new InvalidOperationException("You must have called Initialize before calling Uninitialize");
        }
        InitializationState = InitializationState.Uninitializing;
        for (int num = _dependencySortedList.Count - 1; num >= 0; num--)
        {
            ModuleNode moduleNode = _dependencySortedList[num];
            if (IsInitialized(moduleNode.Module))
            {
                string text = NameForLogging(moduleNode.Module.Uninitialize);
                try
                {
                    moduleNode.Module.Uninitialize(this);
                }
                catch (Exception exception)
                {
                    if (_log.IsErrorEnabled())
                    {
                        _log.Error("Uninitialize action failed for '" + text + "'", exception);
                    }
                    InitializationState = InitializationState.UninitializeFailed;
                    throw;
                }
                _log.Debug("Uninitialize action successful for '{0}'", text);
                _isInitialized.Remove(moduleNode.Module);
            }
        }
        InitializationState = InitializationState.Uninitialized;
        _log.Information("Uninitialization completed");
    }

    public void OnInitComplete()
    {
        EventHandler initComplete = this._initComplete;
        if (initComplete != null)
        {
            InitializationState = InitializationState.InitializeComplete;
            Delegate[] invocationList = initComplete.GetInvocationList();
            EventArgs eventArgs = new EventArgs();
            Delegate[] array = invocationList;
            foreach (Delegate @delegate in array)
            {
                _timeMeters.Start("OnInitComplete " + @delegate.Method.DeclaringType);
                @delegate.DynamicInvoke(this, eventArgs);
                _timeMeters.Stop("OnInitComplete " + @delegate.Method.DeclaringType);
                _initComplete -= (EventHandler)@delegate;
            }
        }
        if (_log.IsDebugEnabled())
        {
            LogRegisteredModules(delegate (string s)
            {
                _log.Debug(s);
            });
        }
    }

    public bool IsInitialized(IInitializableModule module)
    {
        return _isInitialized.Contains(module);
    }

    private static string NameForLogging(Action<InitializationEngine> action)
    {
        return action.Method.Name + " on class " + action.Method.DeclaringType.AssemblyQualifiedName;
    }

    private void LogRegisteredModules(Action<string> output)
    {
        StringBuilder stringBuilder = new StringBuilder();
        StringBuilder stringBuilder2 = stringBuilder;
        StringBuilder stringBuilder3 = stringBuilder2;
        StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(55, 1, stringBuilder2);
        handler.AppendLiteral("Initialization of modules completed, processed modules:");
        handler.AppendFormatted(Environment.NewLine);
        stringBuilder3.Append(ref handler);
        foreach (IInitializableModule module in Modules)
        {
            stringBuilder2 = stringBuilder;
            StringBuilder stringBuilder4 = stringBuilder2;
            handler = new StringBuilder.AppendInterpolatedStringHandler(0, 2, stringBuilder2);
            handler.AppendFormatted(module.GetType().AssemblyQualifiedName);
            handler.AppendFormatted(Environment.NewLine);
            stringBuilder4.Append(ref handler);
        }
        output(stringBuilder.ToString());
    }
}
