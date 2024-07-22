using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace deserialize
{
    public class CalismaSaati
    {
        public int day_of_week { get; set; }
        public bool closed_all_day { get; set; }
        public int? open_hour { get; set; }
        public int? open_minutes { get; set; }
        public int? close_hour { get; set; }
        public int? close_minutes { get; set; }
        public bool open_all_day { get; set; }
    }

    public class InboxDetail
    {
        public bool greeting_enabled { get; set; }
        public string? greeting_message { get; set; }
        public bool working_hours_enabled { get; set; }
        public string? out_of_office_message { get; set; }
        public required List<CalismaSaati> working_hours { get; set; }
    }

    public class AutomationRule
    {
        public string? message { get; set; }
        public bool active { get; set; }
    }


    public class Program
    {
        static List<DateTime> resmi_tatiller = new List<DateTime>
        {
            new DateTime(DateTime.Now.Year, 1, 1),   // Yıl Başı
            new DateTime(DateTime.Now.Year, 4, 23),  // 23 Nisan
            new DateTime(DateTime.Now.Year, 5, 1),   // 1 Mayıs
            new DateTime(DateTime.Now.Year, 8, 30),  // 30 Ağustos
            new DateTime(DateTime.Now.Year, 10, 29), // 29 Ekim
            new DateTime(DateTime.Now.Year, 5, 19),  // 19 Mayıs
            new DateTime(DateTime.Now.Year, 7, 15),  // 15 Temmuz
            new DateTime(DateTime.Now.Year, 4, 9),   // Ramazan
            new DateTime(DateTime.Now.Year, 4, 10),  // Ramazan
            new DateTime(DateTime.Now.Year, 4, 11),  // Ramazan
            new DateTime(DateTime.Now.Year, 6, 15),  // Kurban
            new DateTime(DateTime.Now.Year, 6, 16),  // Kurban
            new DateTime(DateTime.Now.Year, 6, 17),  // Kurban
            new DateTime(DateTime.Now.Year, 6, 18)   // Kurban
        };

        public static void Main()
        {
            string inboxDetailJson = @"{ 
            ""greeting_enabled"": true, 
            ""greeting_message"": ""Merhabalar, UzmanCRM olarak sizlere en kisa zamanda donus yapacagiz."", 
            ""working_hours_enabled"": true, 
            ""out_of_office_message"": ""We are unavailable at the moment. Leave a message we will respond once we are back."", 
            ""working_hours"": [ 
                { 
                    ""day_of_week"": 0, 
                    ""closed_all_day"": true, 
                    ""open_hour"": null, 
                    ""open_minutes"": null, 
                    ""close_hour"": null, 
                    ""close_minutes"": null, 
                    ""open_all_day"": false 
                }, 
                { 
                    ""day_of_week"": 1, 
                    ""closed_all_day"": false, 
                    ""open_hour"": 9, 
                    ""open_minutes"": 0, 
                    ""close_hour"": 17, 
                    ""close_minutes"": 0, 
                    ""open_all_day"": false 
                }, 
                { 
                    ""day_of_week"": 2, 
                    ""closed_all_day"": false, 
                    ""open_hour"": 9, 
                    ""open_minutes"": 0, 
                    ""close_hour"": 17, 
                    ""close_minutes"": 0, 
                    ""open_all_day"": false 
                }, 
                { 
                    ""day_of_week"": 3, 
                    ""closed_all_day"": false, 
                    ""open_hour"": 9, 
                    ""open_minutes"": 0, 
                    ""close_hour"": 17, 
                    ""close_minutes"": 0, 
                    ""open_all_day"": false 
                }, 
                { 
                    ""day_of_week"": 4, 
                    ""closed_all_day"": false, 
                    ""open_hour"": 9, 
                    ""open_minutes"": 0, 
                    ""close_hour"": 17, 
                    ""close_minutes"": 0, 
                    ""open_all_day"": false 
                }, 
                { 
                    ""day_of_week"": 5, 
                    ""closed_all_day"": false, 
                    ""open_hour"": 9, 
                    ""open_minutes"": 0, 
                    ""close_hour"": 17, 
                    ""close_minutes"": 0, 
                    ""open_all_day"": false 
                }, 
                { 
                    ""day_of_week"": 6, 
                    ""closed_all_day"": true, 
                    ""open_hour"": null, 
                    ""open_minutes"": null, 
                    ""close_hour"": null, 
                    ""close_minutes"": null, 
                    ""open_all_day"": false 
                } 
            ] 
        }";

            var automationRuleJsonOpen = @"{ 
            ""message"":""Aktif"", 
            ""active"":true 
        }";
            var automationRuleJsonClose = @"{ 
            ""message"":""Kapalı"", 
            ""active"":true 
        }";

            InboxDetail inboxDetail = JsonSerializer.Deserialize<InboxDetail>(inboxDetailJson);
            AutomationRule automationRuleOpen = JsonSerializer.Deserialize<AutomationRule>(automationRuleJsonOpen);
            AutomationRule automationRuleClose = JsonSerializer.Deserialize<AutomationRule>(automationRuleJsonClose);

            
            Console.WriteLine("Tarih (YYYY-MM-DD):");
            DateTime tarih = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Saat (HH:MM):");
            TimeSpan saat = TimeSpan.Parse(Console.ReadLine());

            DayOfWeek gun = tarih.DayOfWeek;
            bool resmiTatil = resmi_tatiller.Any(rt => rt.Date == tarih.Date);
            var calismaSaati = inboxDetail.working_hours.FirstOrDefault(w => w.day_of_week == (int)gun);

            if (gun == DayOfWeek.Saturday || gun == DayOfWeek.Sunday)
            {
                
                Console.WriteLine(automationRuleClose.message);
                Console.WriteLine(inboxDetail.out_of_office_message);
            }
            else if (resmiTatil)
            {
                
                Console.WriteLine(automationRuleClose.message);
                Console.WriteLine(inboxDetail.out_of_office_message);
            }
            else
            {
                
                if (calismaSaati != null && !calismaSaati.closed_all_day)
                {
                    if (saat >= new TimeSpan(calismaSaati.open_hour ?? 0, calismaSaati.open_minutes ?? 0, 0) &&
                        saat <= new TimeSpan(calismaSaati.close_hour ?? 0, calismaSaati.close_minutes ?? 0, 0))
                    {
                        
                        Console.WriteLine(automationRuleOpen.message);
                        Console.WriteLine(inboxDetail.greeting_message);
                    }
                    else
                    {
                        
                        Console.WriteLine(automationRuleClose.message);
                        Console.WriteLine(inboxDetail.out_of_office_message);
                    }
                }
                else
                {
                    
                    Console.WriteLine(automationRuleClose.message);
                    Console.WriteLine(inboxDetail.out_of_office_message);
                }
            }

        }
    }
}