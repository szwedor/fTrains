using System;
using System.Collections.Generic;
using DomainModel.Models;

namespace Domain
{
   public static class seed
    {
        public static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }


        public static void Seed(TrainContext cont)
        {
            
            {
                User[] users =
                {
                    new User
                    {
                        FirstName = "User",
                        LastName = "User",
                        Email = "duda.buziaczki@gov.pl",
                        PhoneNo = "0700888008",
                        PassWord = GetStringSha256Hash("User1")
                    },
                    new User
                    {
                        FirstName = "Admin",
                        LastName = "Admin",
                        Email = "admin@admin.pl",
                        PhoneNo = "admin",
                        PassWord = GetStringSha256Hash("Admin1")
                    }
                };
                Train[] trainsNames =
                {
                    new Train() {Name = "Kościuszko", SeatNo = 255 ,IsArchival = false},
                    new Train() {Name = "Złoty pociąg", SeatNo = 64,IsArchival = false},
                    new Train() {Name = "Bardziej złoty pociąg", SeatNo = 32,IsArchival = false},
                    new Train() {Name = "Najzłotszy pociąg", SeatNo = 16,IsArchival = false},
                    new Train() {Name = "Nie taki złoty ale też niemiecki", SeatNo = 128,IsArchival = false},
                    new Train() {Name = "Deutschland express", SeatNo = 1000,IsArchival = false},
                    new Train() {Name = "Agnieszka", SeatNo = 3,IsArchival = false},
                    new Train() {Name = "Expres Polarny", SeatNo = 81,IsArchival = false},
                    new Train() {Name = "Expres Subpolarny", SeatNo = 90,IsArchival = false},
                    new Train() {Name = "Polska Kolej Magnetyczna", SeatNo = 42,IsArchival = false},
                    new Train() {Name = "Czarny Piotruś", SeatNo = 39,IsArchival = false},
                    new Train() {Name = "Arycksiąże Ferdynand", SeatNo = 14,IsArchival = false},
                    new Train() {Name = "Tito", SeatNo = 95,IsArchival = false}
                };
                Station[] stationsName =
                {
                    new Station() {Name =  "Warszawa",IsArchival = false},
                    new Station() {Name = "Kraków",IsArchival = false},
                    new Station() {Name = "Rzeszów",IsArchival = false},
                    new Station() {Name = "Poznań",IsArchival = false},
                    new Station() {Name = "Wąchock",IsArchival = false},
                    new Station() {Name = "Władywostok",IsArchival = false},
                    new Station() {Name = "Moskwa",IsArchival = false},
                    new Station() {Name = "Saint Petersburg",IsArchival = false},
                    new Station() {Name = "Leningrad",IsArchival = false},
                    new Station() {Name = "Kursk",IsArchival = false},
                    new Station() {Name = "Budziszyn",IsArchival = false},
                    new Station() {Name = "Berlin",IsArchival = false }
                };
                foreach (var station in stationsName)
                    cont.Stations.Add(station);
                foreach (var train in trainsNames)
                    cont.Trains.Add(train);
                foreach (var user in users)
                    cont.Users.Add(user);

                Random random = new Random();
                List<ConnectionDefinition> lc = new List<ConnectionDefinition>(stationsName.Length * stationsName.Length / 2);
                for (int i = 0; i < stationsName.Length * stationsName.Length * 2; i++)
                {
                    int z = random.Next(stationsName.Length);
                    int y = random.Next((stationsName.Length));

                    int h = random.Next(24);
                    int mm = random.Next(60);
                    DateTime ar = new DateTime(1970, 1, 1, h, mm, 0);
                    h = random.Next(24);
                    mm = random.Next(60);
                    DateTime dr = new DateTime(1970, 1, 1, h, mm, 0);
                    if (ar < dr) ar = ar.AddDays(1);
                    if (y == z) continue;
                    Station a = stationsName[z];
                    Station b = stationsName[y];
                    Train t = trainsNames[random.Next(trainsNames.Length)];
                    int pr = random.Next(200);
                    int pl = t.SeatNo;

                    ConnectionDefinition connection = new ConnectionDefinition()
                    {
                        Arrival = a,
                        Departure = b,
                        Name = b.Name + " " + a.Name,
                        Train = t,
                        Price = pr,
                        TravelTime = ar.Subtract(dr)

                    };
                    //  if (lc.Find(p => p.Name == connection.Name) == null)
                    lc.Add(connection);




                }


                foreach (var con in lc)
                {


                    random = new Random();
                    int min = random.Next(60);
                    int hh = random.Next(24);
                    int days = random.Next(7);
                    DateTime dep = DateTime.Now.AddMinutes(min).AddDays(days).AddHours(hh);
                    con.IsArchival = false;
                    for (int i = 0; i < 52; i++)
                    {
                        Connection c = new Connection();
                        c.ConnectionDefinition = con;
                        c.DepartureTime = dep.AddDays(7 * i);
                        c.ArrivalTime = c.DepartureTime.Add(con.TravelTime);

                        c.AvailableSeatNo = con.Train.SeatNo;
                        cont.Connections.Add(c);
                    }

                    cont.ConnectionDefinitions.Add(con);
                }
                cont.SaveChanges();
            }
        }

    }
}
