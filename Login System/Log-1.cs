using System;
namespace a{
    class User{
        //CREATE THE PROPERTIES
        private string username;
        private string password;

        //CREATE TWO CONSTRUCTORS
        //              DEFAULT CONSTRUCTOR
        public User(){
            this.username="Vhugala";
            this.password="123";
        }

        //              CREATE A CONSTRUCTOR WITH PARAMETERS THAT WE WILL PASS VALUES LATER ON
        public User(string username, string password){
            this.username=username;
            this.password=password;
        }

        // CREATE SETTERS AND GETTERS METHOD THAT WILL ALLOW US TO ACCESS THE PRIVATE ATTRIBUTES
        public void setUsername(string username){
            this.username=username;
        }
        public void setPassword(string password){
            this.password=password;
        }
        public string getUsername(){
            return username;
        }
        public string getPassword(){
            return password;
        }

    }
    class run{
        public static void Main(string [] args){
            //CREATE TWO OBJECTS OF USER THAT STORES DIFFERENT DATA
            User admin=new User("admin", "123");
            User client=new User("client", "123");

            //CREATE A DATA STRUCTURE THAT STORES ALL THE OBJECTS CREATED ABOVE
            //  EXAMPLE 1: ARRAY
            User[] arrayOfUsers = {admin, client};

            //  EXAMPLE 2: LIST
            /* List<User>listOfUsers = new List<User>();
            listOfUsers.add(admin);
            listOfUsers.add(client);
 */
            //LOGIN VALIDATION
            Console.Write("Enter username: ");
            string username=Console.ReadLine();
            Console.Write("Enter password: ");
            string password=Console.ReadLine();

            for(int init = 0; init<arrayOfUsers.Length; init++){
                //CHECKING FOR USER EXISTENCE
                if(arrayOfUsers[init].getUsername()==username && arrayOfUsers[init].getPassword()==password){
                    Console.WriteLine("You are logged in as "+arrayOfUsers[init].getUsername());
                }
            }
        }
    }
}