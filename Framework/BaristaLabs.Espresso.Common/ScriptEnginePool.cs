namespace BaristaLabs.Espresso.Common
{
    using System;
    using System.Collections.Concurrent;

    public class ScriptEnginePool<T>
        where T : IJavaScriptEngine
    {
        private ConcurrentBag<T> m_scriptEngines;
        private Func<T> m_scriptEngineGenerator;

        public ScriptEnginePool(Func<T> scriptEngineGenerator)
        {
            if (scriptEngineGenerator == null) throw new ArgumentNullException("scriptEngineGenerator");
            m_scriptEngines = new ConcurrentBag<T>();
            m_scriptEngineGenerator = scriptEngineGenerator;
        }

        public T GetObject()
        {
            T item;
            if (m_scriptEngines.TryTake(out item))
                return item;

            return m_scriptEngineGenerator();
        }

        public void PutObject(T item)
        {
            m_scriptEngines.Add(item);
        }
    }
}
