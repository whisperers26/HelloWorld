using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClass
{
    public abstract class Pet
    {
        private string name;
        public int age;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public abstract void Eat();

        public abstract void Play();

        public abstract void GotoVet();

        public Pet()
        {
            this.name = "";
            this.age = 0;
        }

        public Pet(string name, int age)
        {
            this.name = name;
            this.age = age;
        }
    }

    public class Pets
    {
        public List<Pet> petList = new List<Pet>();

        public Pet this[int nPetEl]
        {
            get
            {
                Pet returnVal;
                try
                {
                    returnVal = (Pet)petList[nPetEl];
                }
                catch
                {
                    returnVal = null;
                }

                return (returnVal);
            }

            set
            {
                // if the index is less than the number of list elements
                if (nPetEl < petList.Count)
                {
                    // update the existing value at that index
                    petList[nPetEl] = value;
                }
                else
                {
                    // add the value to the list
                    petList.Add(value);
                }
            }
        }

        public int Count
        {
            get => petList.Count;
        }

        public void Add(Pet pet)
        {
            petList.Add(pet);
        }

        public void Remove(Pet pet)
        {
            petList.Remove(pet);
        }

        public void RemoveAt(int petEl)
        {
            petList.RemoveAt(petEl);
        }
    }

    public interface ICat
    {
        void Eat();
        void Play();
        void Scratch();
        void Purr();
    }

    public interface IDog
    {
        void Eat();
        void Play();
        void Bark();
        void NeedWalk();
        void GotoVet();
    }

    public class Cat : Pet, ICat
    {
        public override void Eat()
        {
            Console.WriteLine("{0}: Yuck, I don't like that!", this.Name);
        }

        public override void Play()
        {
            Console.WriteLine("{0}: Where's that mouse...", this.Name);
        }

        public override void GotoVet()
        {
            
        }

        public void Scratch()
        {
            Console.WriteLine("{0}: Hiss!", this.Name);
        }

        public void Purr()
        {
            Console.WriteLine("{0}: purrrrrrrrrrrrrrrrrrrr", this.Name);
        }

        public Cat()
        {

        }

    }

    public class Dog : Pet, IDog
    {
        public string license;

        public override void Eat()
        {
            Console.WriteLine("{0}: Yummy, I will eat anything!", this.Name);
        }

        public override void Play()
        {
            Console.WriteLine("{0}: Throw the ball, throw the ball!", this.Name);
        }

        public override void GotoVet()
        {
            Console.WriteLine("{0}: Whimper, whimper, no vet!", this.Name);
        }

        public void Bark()
        {
            Console.WriteLine("{0}: Woof woof!", this.Name);
        }

        public void NeedWalk()
        {
            Console.WriteLine("{0}: Woof woof, I need to go out.", this.Name);
        }

        public Dog(string license, string name, int age):base(name, age)
        {
            this.license = license;
            this.Name = name;
            this.age = age;
        }
    }
}
