using System;
using System.Collections;
using System.Diagnostics;
using System.Security.Principal;
namespace MysteryMemeware.Helpers
{
    public static class CPUHelper
    {
        #region Private Constants
        public static readonly int LogicalProcessorCount = Environment.ProcessorCount;
        public static readonly IntPtr AffinityAll = GetAffinityAll();
        private static IntPtr GetAffinityAll()
        {
            BitArray affinityBits = new BitArray(IntPtr.Size * 8);

            for (int i = 0; i < LogicalProcessorCount; i++)
            {
                affinityBits[i] = true;
            }

            byte[] affinityBytes = new byte[IntPtr.Size];

            affinityBits.CopyTo(affinityBytes, 0);

            if (Environment.Is64BitProcess)
            {
                long affinityLong = BitConverter.ToInt64(affinityBytes, 0);

                return (IntPtr)affinityLong;
            }
            else
            {
                int affinityInt = BitConverter.ToInt32(affinityBytes, 0);

                return (IntPtr)affinityInt;
            }
        }
        public static bool CurrentProcessIsAdmin = GetCurrentProcessIsAdmin();
        private static bool GetCurrentProcessIsAdmin()
        {
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
                try
                {
                    identity.Dispose();
                }
                catch
                {

                }
                return isAdmin;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region CPU Priority And Affinity
        //Affinity
        public static bool[] GetAffinity()
        {
            IntPtr affinityPointer = ProcessHelper.CurrentProcess.ProcessorAffinity;
            byte[] affinityBytes;
            if (Environment.Is64BitProcess)
            {
                affinityBytes = BitConverter.GetBytes((ulong)affinityPointer);
            }
            else
            {
                affinityBytes = BitConverter.GetBytes((uint)affinityPointer);
            }
            int affinitySize = LogicalProcessorCount;
            bool[] affinity = new bool[affinitySize];
            BitArray bitArray = new BitArray(affinityBytes);
            for (int bitIndex = 0; bitIndex < affinitySize; bitIndex++)
            {
                affinity[bitIndex] = bitArray[bitIndex];
            }
            return affinity;
        }
        public static void SetAffinity(bool[] affinity)
        {
            if (affinity is null)
            {
                throw new Exception("affinity cannot be null.");
            }
            if (affinity.Length is 0)
            {
                throw new Exception("affinity cannot be empty.");
            }
            if (affinity.Length != LogicalProcessorCount)
            {
                throw new Exception("affinity length must be equal to GetLogicalProcessorCount.");
            }
            bool containsTrue = false;
            foreach (bool affinityCPUValue in affinity)
            {
                if (affinityCPUValue)
                {
                    containsTrue = true;
                    break;
                }
            }
            if (!containsTrue)
            {
                throw new Exception("Affinity must allow usage of at least one logical processor.");
            }
            BitArray affinityBits = new BitArray(affinity);
            byte[] affinityBytes = new byte[IntPtr.Size];
            affinityBits.CopyTo(affinityBytes, 0);
            if (Environment.Is64BitProcess)
            {
                ulong affinityULong = BitConverter.ToUInt64(affinityBytes, 0);
                ProcessHelper.CurrentProcess.ProcessorAffinity = (IntPtr)affinityULong;
            }
            else
            {
                uint affinityUInt = BitConverter.ToUInt32(affinityBytes, 0);
                ProcessHelper.CurrentProcess.ProcessorAffinity = (IntPtr)affinityUInt;
            }
        }
        public static void SetAffinityHighest()
        {
            ProcessHelper.CurrentProcess.ProcessorAffinity = AffinityAll;
        }
        //CPU Priority
        public enum Priority
        {
            OnlyWhenIdle,
            BelowNormal,
            Normal,
            AboveNormal,
            High,
            RealTime
        }
        public static void SetPriority(Priority priority)
        {
            if (priority is Priority.OnlyWhenIdle)
            {
                ProcessHelper.CurrentProcess.PriorityBoostEnabled = false;
                ProcessHelper.CurrentProcess.PriorityClass = ProcessPriorityClass.Idle;
            }
            else if (priority is Priority.BelowNormal)
            {
                ProcessHelper.CurrentProcess.PriorityBoostEnabled = false;
                ProcessHelper.CurrentProcess.PriorityClass = ProcessPriorityClass.BelowNormal;
            }
            else if (priority is Priority.Normal)
            {
                ProcessHelper.CurrentProcess.PriorityBoostEnabled = false;
                ProcessHelper.CurrentProcess.PriorityClass = ProcessPriorityClass.Normal;
            }
            else if (priority is Priority.AboveNormal)
            {
                ProcessHelper.CurrentProcess.PriorityBoostEnabled = true;
                ProcessHelper.CurrentProcess.PriorityClass = ProcessPriorityClass.AboveNormal;
            }
            else if (priority is Priority.High)
            {
                ProcessHelper.CurrentProcess.PriorityBoostEnabled = true;
                ProcessHelper.CurrentProcess.PriorityClass = ProcessPriorityClass.High;
            }
            else if (priority is Priority.RealTime)
            {
                if (!CurrentProcessIsAdmin)
                {
                    throw new Exception("Administrator access is required to run with realtime CPU priority.");
                }
                ProcessHelper.CurrentProcess.PriorityBoostEnabled = true;
                ProcessHelper.CurrentProcess.PriorityClass = ProcessPriorityClass.RealTime;
            }
        }
        public static void SetPriority(Priority priority, bool boostWhenFocused)
        {
            ProcessHelper.CurrentProcess.PriorityBoostEnabled = boostWhenFocused;
            if (priority is Priority.OnlyWhenIdle)
            {
                ProcessHelper.CurrentProcess.PriorityClass = ProcessPriorityClass.Idle;
            }
            else if (priority is Priority.BelowNormal)
            {
                ProcessHelper.CurrentProcess.PriorityClass = ProcessPriorityClass.BelowNormal;
            }
            else if (priority is Priority.Normal)
            {
                ProcessHelper.CurrentProcess.PriorityClass = ProcessPriorityClass.Normal;
            }
            else if (priority is Priority.AboveNormal)
            {
                ProcessHelper.CurrentProcess.PriorityClass = ProcessPriorityClass.AboveNormal;
            }
            else if (priority is Priority.High)
            {
                ProcessHelper.CurrentProcess.PriorityClass = ProcessPriorityClass.High;
            }
            else if (priority is Priority.RealTime)
            {
                if (!CurrentProcessIsAdmin)
                {
                    throw new Exception("Administrator access is required to run with realtime CPU priority.");
                }
                ProcessHelper.CurrentProcess.PriorityClass = ProcessPriorityClass.RealTime;
            }
        }
        public static void SetPriorityHighest()
        {
            ProcessHelper.CurrentProcess.PriorityBoostEnabled = true;
            if (CurrentProcessIsAdmin)
            {
                ProcessHelper.CurrentProcess.PriorityClass = ProcessPriorityClass.RealTime;
            }
            else
            {
                ProcessHelper.CurrentProcess.PriorityClass = ProcessPriorityClass.High;
            }
        }
        //CPU Usage
        public static void SetHighest()
        {
            ProcessHelper.CurrentProcess.ProcessorAffinity = AffinityAll;
            ProcessHelper.CurrentProcess.PriorityBoostEnabled = true;
            if (CurrentProcessIsAdmin)
            {
                ProcessHelper.CurrentProcess.PriorityClass = ProcessPriorityClass.RealTime;
            }
            else
            {
                ProcessHelper.CurrentProcess.PriorityClass = ProcessPriorityClass.High;
            }
        }
        #endregion
    }
}