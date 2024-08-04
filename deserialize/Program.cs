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
        public bool active { get; set; }
    }


    public class Program
    {


        public static void Main()
        {
            string inboxDetailJson = $@"
            {{ 
    ""greeting_enabled"": true, 
    ""greeting_message"": ""Merhabalar, UzmanCRM olarak sizlere en kisa zamanda donus yapacagiz."", 
    ""working_hours_enabled"": false, 
    ""out_of_office_message"": ""We are unavailable at the moment. Leave a message we will respond once we are back."", 
    ""working_hours"": [ 
        {{ 
            ""day_of_week"": 0, 
            ""closed_all_day"": true, 
            ""open_hour"": null, 
            ""open_minutes"": null, 
            ""close_hour"": null, 
            ""close_minutes"": null, 
            ""open_all_day"": false 
        }}, 
        {{ 
            ""day_of_week"": 1, 
            ""closed_all_day"": false, 
            ""open_hour"": 9, 
            ""open_minutes"": 0, 
            ""close_hour"": 17, 
            ""close_minutes"": 0, 
            ""open_all_day"": false 
        }}, 
        {{ 
            ""day_of_week"": 2, 
            ""closed_all_day"": false, 
            ""open_hour"": 9, 
            ""open_minutes"": 0, 
            ""close_hour"": 17, 
            ""close_minutes"": 0, 
            ""open_all_day"": false 
        }}, 
        {{ 
            ""day_of_week"": 3, 
            ""closed_all_day"": false, 
            ""open_hour"": 9, 
            ""open_minutes"": 0, 
            ""close_hour"": 17, 
            ""close_minutes"": 0, 
            ""open_all_day"": false 
        }}, 
        {{ 
            ""day_of_week"": 4, 
            ""closed_all_day"": false, 
            ""open_hour"": 9, 
            ""open_minutes"": 0, 
            ""close_hour"": 17, 
            ""close_minutes"": 0, 
            ""open_all_day"": false 
        }}, 
        {{ 
            ""day_of_week"": 5, 
            ""closed_all_day"": false, 
            ""open_hour"": 9, 
            ""open_minutes"": 0, 
            ""close_hour"": 17, 
            ""close_minutes"": 0, 
            ""open_all_day"": false 
        }}, 
        {{ 
            ""day_of_week"": 6, 
            ""closed_all_day"": true, 
            ""open_hour"": null, 
            ""open_minutes"": null, 
            ""close_hour"": null, 
            ""close_minutes"": null, 
            ""open_all_day"": false 
        }} 
    ] 
}}
";



            var automationRuleJsonOpen = $@"
        {{ 
            ""active"":true 
        }}";

            var automationRuleJsonClose = $@"
        {{ 
            ""active"":false
        }}";


            var inboxDetail = JsonSerializer.Deserialize<InboxDetail>(inboxDetailJson);
            var openRule = JsonSerializer.Deserialize<AutomationRule>(automationRuleJsonOpen);
            var closeRule = JsonSerializer.Deserialize<AutomationRule>(automationRuleJsonClose);

            if (inboxDetail == null)
            {
                return;
            }
            // Tarih ve saat bilgisi alma
            Console.Write("Tarih (yyyy-MM-dd): ");
            DateTime date = DateTime.Parse(Console.ReadLine());

            Console.Write("Saat (HH:mm): ");
            TimeSpan time = TimeSpan.Parse(Console.ReadLine());


            int dayOfWeek = (int)date.DayOfWeek;
            var todaysWorkingHours = inboxDetail.working_hours.Find(w => w.day_of_week == dayOfWeek);
            if (todaysWorkingHours == null) 
            { 
                return;
            }
            if (todaysWorkingHours.closed_all_day)
            {
                //Console.WriteLine($"closed_all_day: {todaysWorkingHours.closed_all_day}");
                if(inboxDetail.working_hours_enabled)
                {
                    closeRule.active = true;
                    openRule.active = false;
                    Console.WriteLine($"automationRuleJsonOpen: {openRule.active}");
                    Console.WriteLine($"automationRuleJsonClose: {closeRule.active}");
                   // Console.WriteLine(inboxDetail.out_of_office_message);
                }
                else
                {
                    closeRule.active = false;
                    openRule.active = false;
                    Console.WriteLine($"automationRuleJsonOpen: {openRule.active}");
                    Console.WriteLine($"automationRuleJsonClose: {closeRule.active}");
                }
            }
            else if (!todaysWorkingHours.closed_all_day)
            {
               if(todaysWorkingHours.open_all_day)
                {
                    //Console.WriteLine($"closed_all_day: {todaysWorkingHours.open_all_day}");
                    if (inboxDetail.greeting_enabled)
                    {
                        closeRule.active = false;
                        openRule.active = true;
                        Console.WriteLine($"automationRuleJsonOpen: {openRule.active}");
                        Console.WriteLine($"automationRuleJsonClose: {closeRule.active}");
                        // Console.WriteLine(inboxDetail.greeting_message);
                    }
                    else
                    {
                        closeRule.active = false;
                        openRule.active = false;
                        Console.WriteLine($"automationRuleJsonOpen: {openRule.active}");
                        Console.WriteLine($"automationRuleJsonClose: {closeRule.active}");
                    }
                }
               else if(!todaysWorkingHours.open_all_day)
                {
                    TimeSpan openTime = new TimeSpan(todaysWorkingHours.open_hour.Value, todaysWorkingHours.open_minutes.Value, 0);
                    TimeSpan closeTime = new TimeSpan(todaysWorkingHours.close_hour.Value, todaysWorkingHours.close_minutes.Value, 0);

                    if (time >= openTime && time <= closeTime)
                    {
                        //Console.WriteLine("Çalışma saatleri içindeyiz");
                        if (inboxDetail.greeting_enabled)
                        {
                            closeRule.active = false;
                            openRule.active = true;
                            Console.WriteLine($"automationRuleJsonOpen: {openRule.active}");
                            Console.WriteLine($"automationRuleJsonClose: {closeRule.active}");
                            // Console.WriteLine(inboxDetail.greeting_message);
                        }
                        else
                        {
                            closeRule.active = false;
                            openRule.active = false;
                            Console.WriteLine($"automationRuleJsonOpen: {openRule.active}");
                            Console.WriteLine($"automationRuleJsonClose: {closeRule.active}");
                        }
                    }
                    else
                    {
                        if (inboxDetail.working_hours_enabled)
                        {
                            closeRule.active = true;
                            openRule.active = false;
                            Console.WriteLine($"automationRuleJsonOpen: {openRule.active}");
                            Console.WriteLine($"automationRuleJsonClose: {closeRule.active}");
                            // Console.WriteLine(inboxDetail.out_of_office_message);
                        }
                        else
                        {
                            closeRule.active = false;
                            openRule.active = false;
                            Console.WriteLine($"automationRuleJsonOpen: {openRule.active}");
                            Console.WriteLine($"automationRuleJsonClose: {closeRule.active}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Veri bulunamadı");
                }
            }
            else
            {
                Console.WriteLine("Veri bulunamadı");
            }
        }
    }
}