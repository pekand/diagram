﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diagram
{
    /// <summary>
    /// load plugins</summary>
    public class Plugins //UID8736657869
    {
        public ICollection<IDiagramPlugin> plugins = new List<IDiagramPlugin>();
        public ICollection<INodeOpenPlugin> nodeOpenPlugins = new List<INodeOpenPlugin>();
        public ICollection<IKeyPressPlugin> keyPressPlugins = new List<IKeyPressPlugin>();
        public ICollection<IOpenDiagramPlugin> openDiagramPlugins = new List<IOpenDiagramPlugin>();

        /// <summary>
        /// load plugins from path</summary>
        public void LoadPlugins(string path)
        {
            try
            {
                Program.log.Write("Loading plugins from:" + path);

                IEnumerable<string> dllFileNames = null;
                if (Directory.Exists(path))
                {
                    dllFileNames = Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories);
                }

                ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Count());
                foreach (string dllFile in dllFileNames)
                {
                    try
                    { 
                        AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                        Assembly assembly = Assembly.Load(an);
                        if (assembly != null)
                        {
                            assemblies.Add(assembly);
                        }
                    } 
                    catch (Exception e)
                    {
                        Program.log.Write("Load plugin error: " + dllFile + "  : " + e.Message);
                    }
                }

                // proces all assemblies in folder
                foreach (Assembly assembly in assemblies)
                {
                    Type[] types;

                    try
                    {
                        types = assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        Program.log.Write("Load types from " + assembly.FullName + ": location:"+ assembly.Location + "plugin error  : " + ex.Message);

                        foreach (var item in ex.LoaderExceptions)
                        {                            
                            Program.log.Write("Loaderexceptoon: " + item.Message);
                        }

                        Program.log.Write("Skipping plugin due to errors");
                        continue;
                    }

                    // proces all elements like classes in assemblie
                    foreach (Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }

                        // get all libraries with IDiagramPlugin interface
                        if (type.GetInterface(typeof(IDiagramPlugin).FullName) == null)
                        {
                            continue;
                        }

                        // create plugin instance
                        IDiagramPlugin plugin = Activator.CreateInstance(type) as IDiagramPlugin;
                        if (plugin == null)
                        {
                            continue;
                        }

                        // original assembly location for mapping resources
                        plugin.SetLocation(assembly.Location);

                        // add log object to plugin and allow debug messages from plugin
                        plugin.SetLog(Program.log);

                        // assign plugin to collection of all plugins
                        plugins.Add(plugin);

                        Program.log.Write("Loading plugin: " + plugin.Name);

                        // add plugin to category

                        if (type.GetInterface(typeof(INodeOpenPlugin).FullName) != null)
                        {
                            nodeOpenPlugins.Add(plugin as INodeOpenPlugin);
                        }

                        if (type.GetInterface(typeof(IKeyPressPlugin).FullName) != null)
                        {
                            keyPressPlugins.Add(plugin as IKeyPressPlugin);
                        }

                        if (type.GetInterface(typeof(IOpenDiagramPlugin).FullName) != null)
                        {
                            openDiagramPlugins.Add(plugin as IOpenDiagramPlugin);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Program.log.Write("Load plugin error : " + e.Message);
            }
        }

        /// <summary>
        /// run event for all registred plugins in NodeOpenPlugins </summary>
        public bool ClickOnNodeAction(Diagram diagram, DiagramView diagramView, Node node) 
        {
            bool stopNextAction = false;
                           
            if (nodeOpenPlugins.Count > 0)
            {
                foreach (INodeOpenPlugin plugin in nodeOpenPlugins)
                {
                    try
                    {
                        stopNextAction = plugin.ClickOnNodeAction(diagram, diagramView, node);
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

        /// <summary>
        /// run event for all registred plugins in KeyPressPlugins </summary>
        public bool KeyPressAction(Diagram diagram, DiagramView diagramView, Keys keyData)
        {
            bool stopNextAction = false;

            if (keyPressPlugins.Count > 0)
            {
                foreach (IKeyPressPlugin plugin in keyPressPlugins)
                {
                    try
                    {
                        stopNextAction = plugin.KeyPressAction(diagram, diagramView, keyData);
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

        /// <summary>
        /// run event for all registred plugins in KeyPressPlugins </summary>
        public void OpenDiagramAction(Diagram diagram)
        {
            if (openDiagramPlugins.Count > 0)
            {
                foreach (IOpenDiagramPlugin plugin in openDiagramPlugins)
                {
                    try
                    {
                        plugin.OpenDiagramAction(diagram);

                    }
                    catch (Exception e)
                    {
                        Program.log.Write("Exception in plugin: " + plugin.Name + " : " + e.Message);
                    }
                }
            }
        }
    }
}
