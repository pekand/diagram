using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    /// <summary>
    /// load plugins</summary>
    public class Plugins //UID8736657869
    {
        public ICollection<IDiagramPlugin> plugins = null;
        public ICollection<INodeOpenPlugin> nodeOpenPlugins = null;

        /// <summary>
        /// load plugins from path</summary>
        public void LoadPlugins(string path)
        {
            Program.log.Write("Loading plugins");

            IEnumerable<string> dllFileNames = null;
            if (Directory.Exists(path))
            {
                dllFileNames = Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories);
            }

            ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Count());
            foreach (string dllFile in dllFileNames)
            {
                AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                Assembly assembly = Assembly.Load(an);
                assemblies.Add(assembly);
            }

            Type pluginType = typeof(IDiagramPlugin);            
            ICollection<Type> pluginTypes = new List<Type>();

            Type nodeOpenType = typeof(INodeOpenPlugin);
            ICollection<Type> nodeOpenTypes = new List<Type>();

            // proces all assemblies in folder
            foreach (Assembly assembly in assemblies)
            {
                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {

                            // sort plugins to categories

                            if (type.GetInterface(pluginType.FullName) != null)
                            {
                                pluginTypes.Add(type);
                            }

                            if (type.GetInterface(nodeOpenType.FullName) != null)
                            {
                                nodeOpenTypes.Add(type);
                            }
                        }
                    }
                }
            }

            ///TODO: create one instance for all types of interfaces

            // create instances of plugin
            if (pluginTypes.Count > 0) {
                if (this.plugins == null)
                { 
                    this.plugins = new List<IDiagramPlugin>(pluginTypes.Count);
                }

                foreach (Type type in pluginTypes)
                {
                    IDiagramPlugin plugin = (IDiagramPlugin)Activator.CreateInstance(type);
                    if (plugin != null)
                    {
                        plugins.Add(plugin);

                        Program.log.Write("Loading plugin: " + plugin.Name);
                    }
                }
            }

            // create instances of plugin
            if (nodeOpenTypes.Count > 0)
            {
                if (this.nodeOpenPlugins == null)
                {
                    this.nodeOpenPlugins = new List<INodeOpenPlugin>(nodeOpenTypes.Count);
                }

                foreach (Type type in nodeOpenTypes)
                {
                    INodeOpenPlugin plugin = (INodeOpenPlugin)Activator.CreateInstance(type);
                    if (plugin != null)
                    {
                        nodeOpenPlugins.Add(plugin);
                    }
                }
            }
        }

        /// <summary>
        /// run event for all registred plugins in NodeOpenPlugins </summary>
        public bool ClickOnNodeAction(Diagram diagram, Node node) {
            bool stopNextAction = false;

                           
            if (plugins != null)
            {
                foreach (INodeOpenPlugin plugin in nodeOpenPlugins)
                {
                    try
                    {
                        stopNextAction = plugin.ClickOnNodeAction(diagram, node);
                        if (stopNextAction)
                        {
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Program.log.Write("Exception in plugin: " + plugin.Name + " : " + e.Message);
                    }
                }
            }
            
            return stopNextAction;
        }
    }
}
