namespace BanqueTardi.Utils
{
    public class Utils
    {
        public static string GenererIdentifiantClient(int codeDernierClient)
        {
            int NB_CHIFFRE_ID = 8;

            string id = (codeDernierClient + 1).ToString();

            while (id.Length < NB_CHIFFRE_ID)
            {
                id = "0" + id;
            }

            return $"CL{id}";
        }
    }
}
