using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Classes.Models;
using Newtonsoft.Json.Linq;
using Ultilities.Database;
using Ultilities.Tools;
using Utilities.Database;

namespace Analyse
{
    public class Program
    {
        static void Main(string[] args)
        {
            User loggedUser = new User();
            User user = new User();
            List<Data> userDatas = new List<Data>();

            loggedUser = Login();
            user = SelectUser();
            DisplayData(user);

            //Console.ReadLine();
        }

        public static User Login()
        {
            Console.WriteLine("*** Connexion ***\n");
            User user = null;
            bool allowed = false;
            do
            {
                Console.Write("Identifiant: ");
                string login = Console.ReadLine();
                Console.Write("Mot de passe: ");
                string password = ReadPassword();

                user = new UserManager().GetByLogin(login, password);
                foreach (Role userRole in user.Roles)
                {
                    if (userRole.Name == "admin" || userRole.Name == "command_line")
                    {
                        allowed = true;
                    }
                }

            } while (user.Id == 0 || !allowed);
            Console.Clear();

            return user;
        }

        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password;
        }

        public static User SelectUser()
        {
            User selectedUser = null;
            Console.WriteLine("*** Liste des utilisateurs ***\n");

            List<User> users = LoadUsers().Result;
            foreach (User u in users)
            {
                Console.WriteLine(string.Format($"{u.Id} - {u.Firstname} {u.Lastname}"));
            }

            while (true)
            {
                Console.Write("\nSelectionner un utilisateur (tapez son numéro): ");
                string userId = Console.ReadLine();

                try
                {
                    selectedUser = GetUser(Convert.ToInt32(userId)).Result;
                    break;
                }
                catch (FormatException fe)
                {
                    Console.WriteLine("Tapez un numéro");
                }
                catch (AggregateException ae)
                {
                    Console.WriteLine("Utilisateur invalide");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Une erreur est survenue, réessayez");
                }
            }
            Console.Clear();
           
            return selectedUser;
        }

        public static async void DisplayData(User user)
        {
            Manager<Data> dataManager = new Manager<Data>(ConnectionResource.LocalMySQL);
            Console.WriteLine("Utilisateur sélectionné: " + user.Firstname + " " + user.Lastname);

            int totalData = dataManager.DbSetT.Select(d => d.Id).Count();


            List<Data> userDatas = new List<Data>(LoadUserData(user, 1, 2).Result);

           Console.WriteLine("Données de l'utilisateur " + user.Firstname + " " + user.Lastname);
            foreach (Data userData in userDatas)
            {
                Console.WriteLine(userData.Id + " - " + userData.DataValue);
            }

            Data selectedData;
            while (true)
            {
                Console.Write("Sélectionnez la donnée à modifier: ");
                string dataId = Console.ReadLine();

                try
                {
                    selectedData = await new Manager<Data>(ConnectionResource.LocalMySQL).Get(Convert.ToInt32(dataId));
                    break;
                }
                catch (FormatException fe)
                {
                    Console.WriteLine("Tapez un numéro");
                }
                catch (AggregateException ae)
                {
                    Console.WriteLine("Donnée invalide");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Une erreur est survenue, réessayez");
                }
            }
            Console.Clear();
            Console.WriteLine("Donnée sélectionnée:\n");
            Console.WriteLine(selectedData.DataValue);

            ModifyData(selectedData);
        }

        public static void ModifyData(Data data)
        {
            JObject jsonJObject = JObject.Parse(data.DataValue);

            while (true)
            {
                Console.WriteLine("Que voulez-vous faire:\n");
                Console.WriteLine("1 - Ajouter une clé");
                Console.WriteLine("2 - Modifier une clé");
                Console.WriteLine("3 - Supprimer une clé");
                Console.WriteLine("exit - Quitter\n");

                string response = Console.ReadLine();
                if (response == "1")
                {
                    Console.Write("Insérer avant ou après une clé: ");
                    string whereToInsert = Console.ReadLine();
                    Console.Write("Quelle clé: ");
                    string previousKey = Console.ReadLine();
                    Console.Write("Nouvelle clé: ");
                    string key = Console.ReadLine();
                    Console.Write("La valeur est un tableau ? ");
                    string resp = Console.ReadLine();
                    if (resp == "oui")
                    {
                        if (whereToInsert == "avant")
                        {
                            jsonJObject.Property(previousKey).AddBeforeSelf(new JProperty(key, new JArray()));
                        }
                        else
                        {
                            jsonJObject.Property(previousKey).AddAfterSelf(new JProperty(key, new JArray()));
                        }
                        JArray arrayValue = (JArray) jsonJObject[key];
                        while (true)
                        {
                            Console.Write("Ajouter un item au tableau (ENTRER pour arrêter): ");
                            string item = Console.ReadLine();
                            if (string.IsNullOrEmpty(item))
                            {
                                break;
                            }
                            else
                            {
                                arrayValue.Add(item);
                            }
                        }
                    }
                    else
                    {
                        Console.Write("Valeur de la clé " + key + ": ");
                        string value = Console.ReadLine();
                        if (whereToInsert == "avant")
                        {
                            jsonJObject.Property(previousKey).AddBeforeSelf(new JProperty(key, value));
                        }
                        else
                        {
                            jsonJObject.Property(previousKey).AddAfterSelf(new JProperty(key, value));
                        }
                    }
                }
                else if (response == "2")
                {
                    Console.Write("Clé à modifier: ");
                    string keyToModify = Console.ReadLine();
                    Console.Write("La nouvelle valeur est un tableau ? ");
                    string resp = Console.ReadLine();
                    if (resp == "oui")
                    {
                        JArray arrayValue = new JArray();
                        while (true)
                        {
                            Console.Write("Ajouter un item au tableau (ENTRER pour arrêter): ");
                            string item = Console.ReadLine();
                            if (string.IsNullOrEmpty(item))
                            {
                                jsonJObject[keyToModify] = arrayValue;
                                break;
                            }
                            else
                            {
                                arrayValue.Add(item);
                            }
                        }
                    }
                    else
                    {
                        Console.Write("Nouvelle valeur de la clé " + keyToModify + ": ");
                        string value = Console.ReadLine();
                        jsonJObject[keyToModify] = value;
                    }
                }
                else if (response == "3")
                {
                    Console.Write("Clé à supprimer: ");
                    string keyToDelete = Console.ReadLine();
                    jsonJObject.Property(keyToDelete).Remove();
                }
                else if (response == "exit")
                {
                    break;
                }

                Console.Clear();
                Console.WriteLine("Nouvelle data:");
                Console.WriteLine(jsonJObject.ToString());
                data.DataValue = jsonJObject.ToString();
                if (CommonUtilities.IsValidJson(data.DataValue))
                {
                    UpdateData(data);
                }
            }
        }

        public static async void UpdateData(Data data)
        {
            await new Manager<Data>(ConnectionResource.LocalMySQL).Update(data);
        }

        public static async Task<List<User>> LoadUsers()
        {
            return (await new Manager<User>(ConnectionResource.LocalMySQL).Get()).ToList();
        }

        public static async Task<User> GetUser(int id)
        {
            return (await new UserManager().Get(Convert.ToInt32(id)));
        }

        public static async Task<IEnumerable<Data>> LoadUserData(User user, int page, int limit)
        {
            Manager<Data> dataManager = new Manager<Data>(ConnectionResource.LocalMySQL);
            int skip = limit * (page - 1);

            return (await dataManager.Get()).Where(d => d.UserId == user.Id).Skip(skip).Take(limit);
        }
    }
}
