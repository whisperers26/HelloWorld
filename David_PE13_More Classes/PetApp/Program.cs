using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetClass;

namespace PetApp
{
    //summary
    //test pet class
    //summary
    class Program
    {
        //summary
        //main method
        //summary
        static void Main(string[] args)
        {
            Pet thisPet = null;
            Dog dog = null;
            Cat cat = null;
            IDog iDog = null;
            ICat iCat = null;

            Pets pets = new Pets();

            //create random seed
            Random rand = new Random();

            for(int i = 0; i < 50; i++)
            {
                if (rand.Next(1, 11) == 1)
                {
                    if (rand.Next(0, 2) == 0)
                    {
                        // add a dog
                        string name, inputAge, license;
                        int age;
                        //ask for dog information
                        Console.Write("Dog's Name => ");
                        name = Console.ReadLine();
                        do
                        {
                            Console.Write("Age => ");
                            inputAge = Console.ReadLine();
                            if (int.TryParse(inputAge, out age)) break;
                        } while (true);
                       
                        Console.Write("License => ");
                        license = Console.ReadLine();

                        dog = new Dog(license, name, age);
                        pets.Add(dog);
                    }
                    else
                    {
                        // else add a cat
                        cat = new Cat();
                        //ask for cat information
                        Console.Write("Cat's Name => ");
                        cat.Name = Console.ReadLine();
                        do
                        {
                            Console.Write("Age => ");
                            if (int.TryParse(Console.ReadLine(), out cat.age)) break;
                        } while (true);

                        pets.Add(cat);
                    }
                }
                else
                {
                    // choose a random pet from pets and choose a random activity for the pet to do
                    int ranPet = rand.Next(0, pets.Count);
                    thisPet = pets[ranPet];
                    if (thisPet == null)
                    {
                        continue;
                    }
                    else
                    {
                        if (thisPet.GetType().Equals(typeof(Cat)))
                        {
                            iCat = (ICat)thisPet;
                            switch(rand.Next(0, 4))
                            {
                                case 0:
                                    iCat.Eat();
                                    break;
                                case 1:
                                    iCat.Play();
                                    break;
                                case 2:
                                    iCat.Purr();
                                    break;
                                case 3:
                                    iCat.Scratch();
                                    break;
                                default:
                                    Console.WriteLine("exception!");
                                    break;
                            }
                        }
                        else if (thisPet.GetType().Equals(typeof(Dog)))
                        {
                            iDog = (IDog)thisPet;
                            switch (rand.Next(0, 5))
                            {
                                case 0:
                                    iDog.Eat();
                                    break;
                                case 1:
                                    iDog.Play();
                                    break;
                                case 2:
                                    iDog.Bark();
                                    break;
                                case 3:
                                    iDog.NeedWalk();
                                    break;
                                case 4:
                                    iDog.GotoVet();
                                    break;
                                default:
                                    Console.WriteLine("exception!");
                                    break;
                            }
                        }

                    }

                }

            }
        }
    }
}
