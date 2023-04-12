using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace memento
{
    interface iMemento
    {
        string GetString();
        string GetState();
        DateTime GetDate();
    }
    class Originator
    {
        private string State;
        public Originator(string State)
        {
            this.State = State;
        }
        public void EnterString()
        {
            State = Console.ReadLine();
        }
        public iMemento Save()
        {
            return new ConcreteMemento(State);
        }
        public void Restore(iMemento Memento)
        {
            State = Memento.GetState();
        }
    }
    class ConcreteMemento : iMemento
    {
        private string State;
        private DateTime Date;
        public ConcreteMemento(string State)
        {
            this.State = State;
            Date = DateTime.Now;
        }
        public string GetString()
        {
            return $"{Date} | {State.Substring(0, 9)}";
        }
        public string GetState()
        {
            return State;
        }
        public DateTime GetDate()
        {
            return Date;
        }
    }
    class Caretaker
    {
        private List<iMemento> Arr = new List<iMemento>();
        private Originator Originator;
        public Caretaker(Originator Originator)
        {
            this.Originator = Originator;
        }
        public void Backup()
        {
            Arr.Add(Originator.Save());
        }
        public void Undo()
        {
            if (Arr.Count == 0)
            {
                return;
            }
            var Memento = Arr.Last();
            Arr.Remove(Memento);
            try
            {
                Originator.Restore(Memento);
            }
            catch (Exception)
            {
                this.Undo();
            }
        }
        public void ShowHistory()
        {
            if (Arr.Count > 256)
            {
                int i = 0;
                while (Arr.Count != 256)
                {
                    Arr.RemoveAt(i);
                    i++;
                }
            }
            int Position = 0;
            foreach (var Memento in Arr)
            {
                Position++;
                Console.WriteLine(Position + " | " + Memento.GetString());
            }
        }
        public void Redo()
        {
            int Position = Convert.ToInt32(Console.ReadLine());
            Arr.Insert(Position - 1, (iMemento)Originator);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // Клиентский код.
            Originator originator = new Originator("Super-duper-super-puper-super.");
            Caretaker caretaker = new Caretaker(originator);

            caretaker.Backup();
            caretaker.Undo();
            caretaker.Redo();
            
            caretaker.ShowHistory();

            Console.WriteLine();
        }
    }
}
