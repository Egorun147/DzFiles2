using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dzshka2
{
    internal class Shelf
    {
        private string[] slots;
        private string name;
        public string Name { get { return name; } }
        public int Size { get { return slots.Length; } }
        public Shelf(int Size, string name)
        {
            slots = new string[Size];
            this.name = name;
        }
        public void Place(int userSlot, string itemName)
        {
            slots[userSlot-1] = itemName;
        }
        public string Take(int userSlot)
        {
            string item = slots[userSlot-1];
            slots[userSlot-1] = null;
            return item;
        }
        public string GetItem(int userSlot)
        {
            return slots[userSlot-1];
        }
        public bool IsEmpty(int  userSlot)
        {
            return slots[userSlot-1] == null;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
