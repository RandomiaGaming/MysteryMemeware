namespace MysteryMemeware
{
    public static class CPUHelper
    {
        public static readonly int LogicalProcessorCount = System.Environment.ProcessorCount;
        public static readonly System.IntPtr AffinityAll = GetAffinityAll();
        private static System.IntPtr GetAffinityAll()
        {
            System.Collections.BitArray affinityBits = new System.Collections.BitArray(System.IntPtr.Size * 8);

            for (int i = 0; i < LogicalProcessorCount; i++)
            {
                affinityBits[i] = true;
            }

            byte[] affinityBytes = new byte[System.IntPtr.Size];

            affinityBits.CopyTo(affinityBytes, 0);

            if (System.Environment.Is64BitProcess)
            {
                long affinityLong = System.BitConverter.ToInt64(affinityBytes, 0);

                return (System.IntPtr)affinityLong;
            }
            else
            {
                int affinityInt = System.BitConverter.ToInt32(affinityBytes, 0);

                return (System.IntPtr)affinityInt;
            }
        }
        public static bool[] GetAffinity()
        {
            System.IntPtr affinityPointer = ProcessHelper.CurrentProcess.ProcessorAffinity;
            byte[] affinityBytes;
            if (System.Environment.Is64BitProcess)
            {
                affinityBytes = System.BitConverter.GetBytes((ulong)affinityPointer);
            }
            else
            {
                affinityBytes = System.BitConverter.GetBytes((uint)affinityPointer);
            }
            int affinitySize = LogicalProcessorCount;
            bool[] affinity = new bool[affinitySize];
            System.Collections.BitArray bitArray = new System.Collections.BitArray(affinityBytes);
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
                throw new System.Exception("affinity cannot be null.");
            }
            if (affinity.Length is 0)
            {
                throw new System.Exception("affinity cannot be empty.");
            }
            if (affinity.Length != LogicalProcessorCount)
            {
                throw new System.Exception("affinity length must be equal to LogicalProcessorCount.");
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
                throw new System.Exception("Affinity must allow use of at least one logical processor.");
            }
            System.Collections.BitArray affinityBits = new System.Collections.BitArray(affinity);
            byte[] affinityBytes = new byte[System.IntPtr.Size];
            affinityBits.CopyTo(affinityBytes, 0);
            if (System.Environment.Is64BitProcess)
            {
                ulong affinityULong = System.BitConverter.ToUInt64(affinityBytes, 0);
                ProcessHelper.CurrentProcess.ProcessorAffinity = (System.IntPtr)affinityULong;
            }
            else
            {
                uint affinityUInt = System.BitConverter.ToUInt32(affinityBytes, 0);
                ProcessHelper.CurrentProcess.ProcessorAffinity = (System.IntPtr)affinityUInt;
            }
        }
        public static void SetAffinityHighest()
        {
            ProcessHelper.CurrentProcess.ProcessorAffinity = AffinityAll;
        }
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
                ProcessHelper.CurrentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.Idle;
            }
            else if (priority is Priority.BelowNormal)
            {
                ProcessHelper.CurrentProcess.PriorityBoostEnabled = false;
                ProcessHelper.CurrentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.BelowNormal;
            }
            else if (priority is Priority.Normal)
            {
                ProcessHelper.CurrentProcess.PriorityBoostEnabled = false;
                ProcessHelper.CurrentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.Normal;
            }
            else if (priority is Priority.AboveNormal)
            {
                ProcessHelper.CurrentProcess.PriorityBoostEnabled = true;
                ProcessHelper.CurrentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.AboveNormal;
            }
            else if (priority is Priority.High)
            {
                ProcessHelper.CurrentProcess.PriorityBoostEnabled = true;
                ProcessHelper.CurrentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
            }
            else if (priority is Priority.RealTime)
            {
                if (!UACHelper.CurrentProcessIsAdmin)
                {
                    throw new System.Exception("Administrator access is required to run with realtime CPU priority.");
                }
                ProcessHelper.CurrentProcess.PriorityBoostEnabled = true;
                ProcessHelper.CurrentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            }
        }
        public static void SetPriority(Priority priority, bool boostWhenFocused)
        {
            ProcessHelper.CurrentProcess.PriorityBoostEnabled = boostWhenFocused;
            if (priority is Priority.OnlyWhenIdle)
            {
                ProcessHelper.CurrentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.Idle;
            }
            else if (priority is Priority.BelowNormal)
            {
                ProcessHelper.CurrentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.BelowNormal;
            }
            else if (priority is Priority.Normal)
            {
                ProcessHelper.CurrentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.Normal;
            }
            else if (priority is Priority.AboveNormal)
            {
                ProcessHelper.CurrentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.AboveNormal;
            }
            else if (priority is Priority.High)
            {
                ProcessHelper.CurrentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
            }
            else if (priority is Priority.RealTime)
            {
                if (!UACHelper.CurrentProcessIsAdmin)
                {
                    throw new System.Exception("Administrator access is required to run with realtime CPU priority.");
                }
                ProcessHelper.CurrentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            }
        }
        public static void SetPriorityHighest()
        {
            ProcessHelper.CurrentProcess.PriorityBoostEnabled = true;
            if (UACHelper.CurrentProcessIsAdmin)
            {
                ProcessHelper.CurrentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            }
            else
            {
                ProcessHelper.CurrentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
            }
        }
        public static void SetUseageHighest()
        {
            ProcessHelper.CurrentProcess.ProcessorAffinity = AffinityAll;
            ProcessHelper.CurrentProcess.PriorityBoostEnabled = true;
            if (UACHelper.CurrentProcessIsAdmin)
            {
                ProcessHelper.CurrentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            }
            else
            {
                ProcessHelper.CurrentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
            }
        }
    }
}