using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Assets.Scripts.Misc.Pipeline
{
    /// <summary>
    /// Base class for Pipeline implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pipeline<T> : IPipeline<T> where T : class
    {
        protected static List<IPipelineProcess<T>> _pipeList;

        protected virtual void BuildPipelineLibrary()
        {
            _pipeList = new List<IPipelineProcess<T>>();

            // Loads all processors types from directory translator
            var translatorTypes = new List<Type>();
            translatorTypes.AddRange(
                Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && typeof(IPipelineProcess<T>).IsAssignableFrom(t))
            );

            // Initializes all actions into dictionary
            foreach (var translatorType in translatorTypes)
            {
                var action = (IPipelineProcess<T>)Activator.CreateInstance(translatorType);

                // Adds only if enabled
                if (action.IsEnabled)
                {
                    _pipeList.Add(action);
                }
            }

            // Orders by priority
            _pipeList = _pipeList.OrderBy(x => x.PriorityId).ToList();
        }

        public virtual void RunThroughPipeline(T data)
        {
            // Rebuilds library on fail
            if (_pipeList == null || _pipeList.Count == 0)
                BuildPipelineLibrary();

            // Runs through out all translators
            _pipeList?.Aggregate(data, (current, processor) => processor.Translate(current));
        }
    }
}
