using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using AutoDumper.ViewModels;
using Autofac;
using Autofac.Features.ResolveAnything;
using Caliburn.Micro;

namespace AutoDumper
{
    public class AppBootstrapper : BootstrapperBase
    {
        public AppBootstrapper()
        {
            Initialize();
        }
        
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override void Configure()
        { 
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray()).
                Where(x => x.Name.EndsWith("ViewModel")).
                Where(x => !(string.IsNullOrWhiteSpace(x.Namespace)) && x.Namespace.EndsWith("ViewModels")).
                Where(x => x.GetInterface(typeof (INotifyPropertyChanged).Name) != null).
                AsSelf().
                InstancePerDependency();

            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray()).
                Where(x => x.Name.EndsWith("View")).
                Where(x => !(string.IsNullOrWhiteSpace(x.Namespace)) && x.Namespace.EndsWith("Views")).
                AsSelf().
                InstancePerDependency();

            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            builder.Register<IWindowManager>(c => new WindowManager()).InstancePerLifetimeScope();
         
            builder.Register<IEventAggregator>(c => new EventAggregator()).InstancePerLifetimeScope();

            _container = builder.Build();
        }

        
        
        protected override object GetInstance(Type serviceType, string key)
        {
            if (!String.IsNullOrWhiteSpace(key))
            {
                object instance = null;
                if (Container.TryResolveKeyed(key, serviceType, out instance))
                {
                    return instance;
                }
            }

            if (Container.IsRegistered(serviceType))
            {
                return Container.Resolve(serviceType);
            }

            throw new Exception($"Could not locate any instances of contract {key ?? serviceType.Name}.");
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            
            return Container.Resolve(typeof(IEnumerable<>).MakeGenericType(serviceType)) as IEnumerable<object>;
        }

        private Autofac.IContainer _container;
        protected Autofac.IContainer Container => _container;
    }
}
