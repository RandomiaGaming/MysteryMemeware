using System;
using System.Threading;
namespace MysteryMemeware.Helpers
{
    public static class ThreadingHelper
    {
        #region Worker Count
        public static int GetOptimalWorkerCount()
        {
            bool[] affinity = CPUHelper.GetAffinity();
            int output = 0;
            foreach (bool coreState in affinity)
            {
                if (coreState)
                {
                    output++;
                }
            }
            return output;
        }
        #endregion
        #region Lambdas
        public delegate void Lambda();
        public delegate void LambdaCompletedEvent(int completedLambdaCount, int totalLambdaCount);
        public delegate void LambdaFailedEvent(Exception exception, int completedLambdaCount, int totalLambdaCount);
        //Note: The thread is already started before returning.
        public static Thread RunLambda(Lambda lambda)
        {
            if (lambda is null)
            {
                throw new Exception("lambda cannot be null.");
            }
            Thread worker = new Thread(() =>
            {
                lambda.Invoke();
            });
            worker.Start();
            return worker;
        }
        //Note: For best preformance sort parameters so that the the slowest most computationally expensive parameters are first.
        public static void RunLambdas(Lambda[] lambdas, LambdaCompletedEvent completedEvent = null, LambdaFailedEvent failedEvent = null, int workerCount = -1, int sleepTime = 0)
        {
            if (lambdas is null)
            {
                throw new Exception("lambdas cannot be null.");
            }
            else if (workerCount <= 0)
            {
                throw new Exception("workerCount must be greater than 0 or equal to -1.");
            }
            else if (sleepTime < 0)
            {
                throw new Exception("sleepTime must be greater than or equal to 0.");
            }
            else if (lambdas.Length is 0)
            {
                return;
            }
            int totalLambdaCount = lambdas.Length;
            int optimalWorkerCount = GetOptimalWorkerCount();
            if (workerCount is -1)
            {
                if (optimalWorkerCount > totalLambdaCount)
                {
                    workerCount = totalLambdaCount;
                }
                else
                {
                    workerCount = optimalWorkerCount;
                }
            }
            else if (workerCount > totalLambdaCount)
            {
                workerCount = totalLambdaCount;
            }
            lambdas = (Lambda[])lambdas.Clone();
            foreach (Lambda lambda in lambdas)
            {
                if (lambda is null)
                {
                    throw new Exception("lambdaGroup cannot contain null.");
                }
            }
            object queLock = new object();
            Thread[] workers = new Thread[workerCount];
            int nextLambdaIndex = 0;
            int completedLambdaCount = 0;
            Lambda mainThreadLambda;
            lock (queLock)
            {
                for (int workerIndex = 0; workerIndex < workerCount; workerIndex++)
                {
                    Lambda workerThreadLambda = lambdas[workerIndex];
                    Thread workerThread = new Thread(() =>
                    {
                        if (completedEvent is null)
                        {
                            if (failedEvent is null)
                            {
                                while (true)
                                {
                                    try
                                    {
                                        workerThreadLambda.Invoke();
                                    }
                                    catch
                                    {

                                    }
                                    lock (queLock)
                                    {
                                        completedLambdaCount++;
                                        if (nextLambdaIndex == totalLambdaCount)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            workerThreadLambda = lambdas[nextLambdaIndex];
                                            nextLambdaIndex++;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                while (true)
                                {
                                    try
                                    {
                                        workerThreadLambda.Invoke();
                                        lock (queLock)
                                        {
                                            completedLambdaCount++;

                                            if (nextLambdaIndex == totalLambdaCount)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                workerThreadLambda = lambdas[nextLambdaIndex];
                                                nextLambdaIndex++;
                                            }
                                        }
                                    }
                                    catch (Exception exception)
                                    {
                                        lock (queLock)
                                        {
                                            completedLambdaCount++;
                                            failedEvent.Invoke(exception, completedLambdaCount, totalLambdaCount);
                                            if (nextLambdaIndex == totalLambdaCount)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                workerThreadLambda = lambdas[nextLambdaIndex];
                                                nextLambdaIndex++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (failedEvent is null)
                        {
                            while (true)
                            {
                                try
                                {
                                    workerThreadLambda.Invoke();
                                }
                                catch
                                {

                                }
                                lock (queLock)
                                {
                                    completedLambdaCount++;
                                    completedEvent.Invoke(completedLambdaCount, totalLambdaCount);
                                    if (nextLambdaIndex == totalLambdaCount)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        workerThreadLambda = lambdas[nextLambdaIndex];
                                        nextLambdaIndex++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            while (true)
                            {
                                try
                                {
                                    workerThreadLambda.Invoke();
                                    lock (queLock)
                                    {
                                        completedLambdaCount++;
                                        completedEvent.Invoke(completedLambdaCount, totalLambdaCount);
                                        if (nextLambdaIndex == totalLambdaCount)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            workerThreadLambda = lambdas[nextLambdaIndex];
                                            nextLambdaIndex++;
                                        }
                                    }
                                }
                                catch (Exception exception)
                                {
                                    lock (queLock)
                                    {
                                        completedLambdaCount++;
                                        failedEvent.Invoke(exception, completedLambdaCount, totalLambdaCount);
                                        if (nextLambdaIndex == totalLambdaCount)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            workerThreadLambda = lambdas[nextLambdaIndex];
                                            nextLambdaIndex++;
                                        }
                                    }
                                }
                            }
                        }
                    });
                    workerThread.Name = "Lambda Worker Thread";
                    workerThread.Start();
                    workers[workerIndex] = workerThread;
                }
                mainThreadLambda = lambdas[workerCount];
                nextLambdaIndex = workerCount + 1;
            }
            if (completedEvent is null)
            {
                if (failedEvent is null)
                {
                    while (true)
                    {
                        try
                        {
                            mainThreadLambda.Invoke();
                        }
                        catch
                        {

                        }
                        lock (queLock)
                        {
                            completedLambdaCount++;
                            if (nextLambdaIndex == totalLambdaCount)
                            {
                                break;
                            }
                            else
                            {
                                mainThreadLambda = lambdas[nextLambdaIndex];
                                nextLambdaIndex++;
                            }
                        }
                    }
                }
                else
                {
                    while (true)
                    {
                        try
                        {
                            mainThreadLambda.Invoke();
                            lock (queLock)
                            {
                                completedLambdaCount++;
                                if (nextLambdaIndex == totalLambdaCount)
                                {
                                    break;
                                }
                                else
                                {
                                    mainThreadLambda = lambdas[nextLambdaIndex];
                                    nextLambdaIndex++;
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            lock (queLock)
                            {
                                completedLambdaCount++;
                                failedEvent.Invoke(exception, completedLambdaCount, totalLambdaCount);
                                if (nextLambdaIndex == totalLambdaCount)
                                {
                                    break;
                                }
                                else
                                {
                                    mainThreadLambda = lambdas[nextLambdaIndex];
                                    nextLambdaIndex++;
                                }
                            }
                        }
                    }
                }
            }
            else if (failedEvent is null)
            {
                while (true)
                {
                    try
                    {
                        mainThreadLambda.Invoke();
                    }
                    catch
                    {

                    }
                    lock (queLock)
                    {
                        completedLambdaCount++;
                        completedEvent.Invoke(completedLambdaCount, totalLambdaCount);
                        if (nextLambdaIndex == totalLambdaCount)
                        {
                            break;
                        }
                        else
                        {
                            mainThreadLambda = lambdas[nextLambdaIndex];
                            nextLambdaIndex++;
                        }
                    }
                }
            }
            else
            {
                while (true)
                {
                    try
                    {
                        mainThreadLambda.Invoke();
                        lock (queLock)
                        {
                            completedLambdaCount++;
                            completedEvent.Invoke(completedLambdaCount, totalLambdaCount);
                            if (nextLambdaIndex == totalLambdaCount)
                            {
                                break;
                            }
                            else
                            {
                                mainThreadLambda = lambdas[nextLambdaIndex];
                                nextLambdaIndex++;
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        lock (queLock)
                        {
                            completedLambdaCount++;
                            failedEvent.Invoke(exception, completedLambdaCount, totalLambdaCount);
                            if (nextLambdaIndex == totalLambdaCount)
                            {
                                break;
                            }
                            else
                            {
                                mainThreadLambda = lambdas[nextLambdaIndex];
                                nextLambdaIndex++;
                            }
                        }
                    }
                }
            }
            if (sleepTime is 0)
            {
                while (completedLambdaCount != totalLambdaCount)
                {

                }
            }
            else
            {
                while (completedLambdaCount != totalLambdaCount)
                {
                    Thread.Sleep(sleepTime);
                }
            }
        }
        #endregion
        #region Param Lambdas
        public delegate void ParamLambda<T>(T parameter);
        public delegate void ParamLambdaCompletedEvent<T>(T parameter, int completedParameterCount, int totalParameterCount);
        public delegate void ParamLambdaFailedEvent<T>(T parameter, Exception exception, int completedParameterCount, int totalParameterCount);
        //Note: The thread is already started before returning.
        public static Thread RunParamLambda<T>(ParamLambda<T> paramLambda, T parameter)
        {
            if (paramLambda is null)
            {
                throw new Exception("paramLambda cannot be null.");
            }
            Thread worker = new Thread(() =>
            {
                paramLambda.Invoke(parameter);
            });
            worker.Start();
            return worker;
        }
        //Note: For best preformance sort parameters so that the the slowest most computationally expensive parameters are first.
        public static void RunParamLambdas<T>(ParamLambda<T> paramLambda, T[] parameters, ParamLambdaCompletedEvent<T> completedEvent = null, ParamLambdaFailedEvent<T> failedEvent = null, int workerCount = -1, int sleepTime = 100)
        {
            if (paramLambda is null)
            {
                throw new Exception("paramLambda cannot be null.");
            }
            else if (parameters is null)
            {
                throw new Exception("parameters cannot be null.");
            }
            else if (workerCount < -1 || workerCount is 0)
            {
                throw new Exception("workerCount must be greater than 0 or equal to -1.");
            }
            else if (sleepTime < 0)
            {
                throw new Exception("sleepTime must be greater than or equal to 0.");
            }
            else if (parameters.Length is 0)
            {
                return;
            }
            int totalParameterCount = parameters.Length;
            int optimalWorkerCount = GetOptimalWorkerCount();
            if (workerCount is -1)
            {
                if (optimalWorkerCount > totalParameterCount)
                {
                    workerCount = totalParameterCount;
                }
                else
                {
                    workerCount = optimalWorkerCount;
                }
            }
            else if (workerCount > totalParameterCount)
            {
                workerCount = totalParameterCount;
            }
            parameters = (T[])parameters.Clone();
            object queLock = new object();
            Thread[] workerThreads = new Thread[workerCount];
            int nextParameterIndex = 0;
            int completedParameterCount = 0;
            T mainThreadParameter;
            lock (queLock)
            {
                for (int workerIndex = 0; workerIndex < workerCount; workerIndex++)
                {
                    T workerThreadParameter = parameters[workerIndex];
                    Thread workerThread = new Thread(() =>
                    {
                        if (completedEvent is null)
                        {
                            if (failedEvent is null)
                            {
                                while (true)
                                {
                                    try
                                    {
                                        paramLambda.Invoke(workerThreadParameter);
                                    }
                                    catch
                                    {

                                    }
                                    lock (queLock)
                                    {
                                        completedParameterCount++;
                                        if (nextParameterIndex == totalParameterCount)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            workerThreadParameter = parameters[nextParameterIndex];
                                            nextParameterIndex++;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                while (true)
                                {
                                    try
                                    {
                                        paramLambda.Invoke(workerThreadParameter);
                                        lock (queLock)
                                        {
                                            completedParameterCount++;

                                            if (nextParameterIndex == totalParameterCount)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                workerThreadParameter = parameters[nextParameterIndex];
                                                nextParameterIndex++;
                                            }
                                        }
                                    }
                                    catch (Exception exception)
                                    {
                                        lock (queLock)
                                        {
                                            completedParameterCount++;
                                            failedEvent.Invoke(workerThreadParameter, exception, completedParameterCount, totalParameterCount);
                                            if (nextParameterIndex == totalParameterCount)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                workerThreadParameter = parameters[nextParameterIndex];
                                                nextParameterIndex++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (failedEvent is null)
                        {
                            while (true)
                            {
                                try
                                {
                                    paramLambda.Invoke(workerThreadParameter);
                                }
                                catch
                                {

                                }
                                lock (queLock)
                                {
                                    completedParameterCount++;
                                    completedEvent.Invoke(workerThreadParameter, completedParameterCount, totalParameterCount);
                                    if (nextParameterIndex == totalParameterCount)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        workerThreadParameter = parameters[nextParameterIndex];
                                        nextParameterIndex++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            while (true)
                            {
                                try
                                {
                                    paramLambda.Invoke(workerThreadParameter);
                                    lock (queLock)
                                    {
                                        completedParameterCount++;
                                        completedEvent.Invoke(workerThreadParameter, completedParameterCount, totalParameterCount);
                                        if (nextParameterIndex == totalParameterCount)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            workerThreadParameter = parameters[nextParameterIndex];
                                            nextParameterIndex++;
                                        }
                                    }
                                }
                                catch (Exception exception)
                                {
                                    lock (queLock)
                                    {
                                        completedParameterCount++;
                                        failedEvent.Invoke(workerThreadParameter, exception, completedParameterCount, totalParameterCount);
                                        if (nextParameterIndex == totalParameterCount)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            workerThreadParameter = parameters[nextParameterIndex];
                                            nextParameterIndex++;
                                        }
                                    }
                                }
                            }
                        }
                    });
                    workerThread.Name = "Lambda Worker Thread";
                    workerThread.Start();
                    workerThreads[workerIndex] = workerThread;
                }
                mainThreadParameter = parameters[workerCount];
                nextParameterIndex = workerCount + 1;
            }
            if (completedEvent is null)
            {
                if (failedEvent is null)
                {
                    while (true)
                    {
                        try
                        {
                            paramLambda.Invoke(mainThreadParameter);
                        }
                        catch
                        {

                        }
                        lock (queLock)
                        {
                            completedParameterCount++;
                            if (nextParameterIndex == totalParameterCount)
                            {
                                break;
                            }
                            else
                            {
                                mainThreadParameter = parameters[nextParameterIndex];
                                nextParameterIndex++;
                            }
                        }
                    }
                }
                else
                {
                    while (true)
                    {
                        try
                        {
                            paramLambda.Invoke(mainThreadParameter);
                            lock (queLock)
                            {
                                completedParameterCount++;

                                if (nextParameterIndex == totalParameterCount)
                                {
                                    break;
                                }
                                else
                                {
                                    mainThreadParameter = parameters[nextParameterIndex];
                                    nextParameterIndex++;
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            lock (queLock)
                            {
                                completedParameterCount++;
                                failedEvent.Invoke(mainThreadParameter, exception, completedParameterCount, totalParameterCount);
                                if (nextParameterIndex == totalParameterCount)
                                {
                                    break;
                                }
                                else
                                {
                                    mainThreadParameter = parameters[nextParameterIndex];
                                    nextParameterIndex++;
                                }
                            }
                        }
                    }
                }
            }
            else if (failedEvent is null)
            {
                while (true)
                {
                    try
                    {
                        paramLambda.Invoke(mainThreadParameter);
                    }
                    catch
                    {

                    }
                    lock (queLock)
                    {
                        completedParameterCount++;
                        completedEvent.Invoke(mainThreadParameter, completedParameterCount, totalParameterCount);
                        if (nextParameterIndex == totalParameterCount)
                        {
                            break;
                        }
                        else
                        {
                            mainThreadParameter = parameters[nextParameterIndex];
                            nextParameterIndex++;
                        }
                    }
                }
            }
            else
            {
                while (true)
                {
                    try
                    {
                        paramLambda.Invoke(mainThreadParameter);
                        lock (queLock)
                        {
                            completedParameterCount++;
                            completedEvent.Invoke(mainThreadParameter, completedParameterCount, totalParameterCount);
                            if (nextParameterIndex == totalParameterCount)
                            {
                                break;
                            }
                            else
                            {
                                mainThreadParameter = parameters[nextParameterIndex];
                                nextParameterIndex++;
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        lock (queLock)
                        {
                            completedParameterCount++;
                            failedEvent.Invoke(mainThreadParameter, exception, completedParameterCount, totalParameterCount);
                            if (nextParameterIndex == totalParameterCount)
                            {
                                break;
                            }
                            else
                            {
                                mainThreadParameter = parameters[nextParameterIndex];
                                nextParameterIndex++;
                            }
                        }
                    }
                }
            }
            if (sleepTime is 0)
            {
                while (completedParameterCount != totalParameterCount)
                {

                }
            }
            else
            {
                while (completedParameterCount != totalParameterCount)
                {
                    Thread.Sleep(sleepTime);
                }
            }
        }
        #endregion
    }
}