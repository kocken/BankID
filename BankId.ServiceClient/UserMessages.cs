namespace BankId.ServiceClient
{
    public static class UserMessages
    {
        public static string RFA1 { get { return "Starta BankID-appen"; } }
        public static string RFA2 { get { return "Du har inte BankID-appen installerad. Kontakta din internetbank."; } }
        public static string RFA3 { get { return "Åtgärden avbruten. Försök igen."; } }
        public static string RFA4 { get { return "En identifiering eller underskrift för det här personnumret är redan påbörjad. Försök igen."; } }
        public static string RFA5 { get { return "Internt tekniskt fel. Försök igen."; } }
        public static string RFA6 { get { return "Åtgärden avbruten."; } }
        // RFA7 doesn't exist
        public static string RFA8 { get { return "BankID-appen svarar inte. Kontrollera att den är startad och att du har internetanslutning. Om du inte har något giltigt BankID kan du hämta ett hos din Bank. Försök sedan igen."; } }
        public static string RFA9 { get { return "Skriv in din säkerhetskod i BankID-appen och välj Identifiera eller Skriv under."; } }
        // RFA10-12 doesn't exist
        public static string RFA13 { get { return "Försöker starta BankID-appen..."; } }
        // The recommended RFA14 A/B & RFA15 A/B messages are combined as a single & shorter custom message
        public static string RFA14AB15AB { get { return "Söker efter BankID, det kan ta en liten stund..."; } }
        public static string RFA16 { get { return "Det BankID du försöker använda är för gammalt eller spärrat. Använd ett annat BankID eller hämta ett nytt hos din internetbank."; } }
        public static string RFA17A { get { return "BankID-appen verkar inte finnas i din dator eller telefon. Installera den och hämta ett BankID hos din internetbank. Installera appen från din appbutik eller https://install.bankid.com."; } }
        public static string RFA17B { get { return "Misslyckades att läsa av QR koden. Starta BankID-appen och läs av QR-koden. Om du inte har BankID-appen måste du installera den och hämta ett BankID hos din internetbank. Installera appen från din appbutik eller https://install.bankid.com."; } }
        public static string RFA18 { get { return "Starta BankID-appen"; } }
        public static string RFA19 { get { return "Vill du identifiera dig eller skriva under med BankID på den här datorn eller med ett Mobilt BankID?"; } }
        public static string RFA20 { get { return "Vill du identifiera dig eller skriva under med ett BankID på den här enheten eller med ett BankID på en annan enhet?"; } }
        public static string RFA21 { get { return "Identifiering eller underskrift pågår."; } }
        public static string RFA22 { get { return "Okänt fel. Försök igen."; } }

        public static string UNKNOWN_STATUS { get { return "Okänd status. Försök igen."; } }
        public static string SUCCESSFUL_AUTHENTICATION { get { return "Autentiseringen lyckades."; } }
    }
}
