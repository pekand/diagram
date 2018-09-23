using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    public class DataStorage
    {
        private IDictionary<string, IStorage> dataStorage = new Dictionary<string, IStorage>(); // data storage

        public IStorage getStorage(string name)
        {
            if (dataStorage.ContainsKey(name)) {
                return dataStorage[name];
            }

            return null;
        }

        public void setStorage(string name, IStorage storage)
        {
            dataStorage[name] = storage;
        }
    }
}
