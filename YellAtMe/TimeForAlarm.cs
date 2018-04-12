﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace YellAtMe
{
    public abstract class TimeForAlarm
    {
        protected DateTime Alarm;
        private bool Triggered = false;
        public string AlarmType { get; set; }
        public string AlarmTime { get; set; }
        public int ID { get; set; }
        

        public bool getTriggered()
        {
            return Triggered;
        }

        public void AlarmWentOff()
        {
            Triggered = true;
            //Console.WriteLine("AlarmWentOff");
            var timer = new Timer();
            timer.Interval = 60000;
            timer.Elapsed += ResetTrigger;
            timer.Start();
        }

        private void ResetTrigger(object sender, ElapsedEventArgs e)
        {
            //Console.WriteLine("ResetTrigger");
            Triggered = false;
            ((Timer)sender).Stop();
        }

        public abstract bool AlarmTriggered();
    }

    public class DailyAlarm : TimeForAlarm
    {
        public DailyAlarm(int hour, int minuite)
        {
            AlarmType = "Daily";
            
            SetTime(hour, minuite);
        }

        public void SetTime(int hour, int minuite)
        {
            Alarm = new DateTime(1, 1, 1, hour, minuite, 0);
            AlarmTime = "Everyday at " + Alarm.ToShortTimeString();
        }


        public override bool AlarmTriggered()
        {
            var now = DateTime.Now;
            if (Alarm.Hour == now.Hour && Alarm.Minute == now.Minute)
                return true;
            return false;
        }
    }

    public class WeeklyAlarm : TimeForAlarm
    {
        private List<DayOfWeek> Days;

        public WeeklyAlarm(List<DayOfWeek> days, int hour, int minuites)
        {
            AlarmType = "Weekly";
            SetTime(days, hour, minuites);
        }

        public void SetTime(List<DayOfWeek> days, int hour, int minuite)
        {
            Days = days;
            Alarm = new DateTime(1, 1, 1, hour, minuite, 0);
            AlarmTime = Alarm.ToShortTimeString() + " on " + String.Join(", " , days.OrderBy(x => x).Select(x => x.ToString()));
        }

        public override bool AlarmTriggered()
        {
            var now = DateTime.Now;
            if (Alarm.Hour == now.Hour && Alarm.Minute == Alarm.Minute &&
                Days.Contains(now.DayOfWeek))
                return true;
            return false;
        }

    }

    public class RandomAlarm : TimeForAlarm
    {

        public RandomAlarm(int year, int month, int day, int hour, int minuite)
        {
            AlarmType = "Random";
            SetAlarm(year, month, day, hour, minuite);
        }

        public void SetAlarm(int year, int month, int day, int hour, int minuite)
        {
            Alarm = new DateTime(year, month, day, hour, minuite, 0);
            AlarmTime = "Alarm will go off at " + Alarm.ToShortDateString() + " "+ Alarm.ToShortTimeString();
        }

        public override bool AlarmTriggered()
        {
            var now = DateTime.Now;
            if (Alarm.Year == now.Year && Alarm.Month == now.Month &&
                Alarm.Day == now.Day && Alarm.Hour == now.Hour &&
                Alarm.Minute == now.Minute)
                return true;
            return false;
        }
    }

}