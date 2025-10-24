using System.Security.Cryptography;


public class Password {
        
        private static int ITERATION = 100000;
        private static int SALT_SIZE = 16;
        private static int OUTPUT_LEN= 32;

        public static void Policies(string psw) {
        }
}


/*
        public static byte[] Hash(string psw) {
                byte[] salt = new Byte[SALT_SIZE]; 
                new Random().NextBytes(salt);
  
        }
        
        public static byte[] Hash(byte[] psw, byte[] salt) {
                //create key
                var key = Rfc2898DeriveBytes.Pbkdf2(psw,
                                                    salt,
                                                    ITERATION,
                                                    HashAlgorithmName.SHA256,
                                                    OUTPUT_LEN);

                
                

                
        }


        public static bool Validate(string psw, string target) {
                

        }
*/

