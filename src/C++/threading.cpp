#include <cstdio>
#include <cstdlib>
#include <stdio.h>
#include <stdlib.h>
#include <iostream>
#include <list>
#include <thread>


using namespace std;

class ThreadPool
{
public:
    int threadAmount;
    std::list<thread&> threads;
    function<void()> processingDelegate;
    void startThreads(function<void()> processingDelegate)
    {
        this->processingDelegate = processingDelegate;
        threads = {};
        for(int i; i < threads.size(); i++)
        {
            thread t= new thread(&(this->doWork));
            threads.push_front(t);
        }
    }
    void doWork()
    {
        while(!shuttingDown)
        {
           try
           {
               this->processingDelegate();
           }
           catch(...){}
        }
    }
    void replaceThread()
    {
        
    }
    void Shutdown()
    {
        shuttingDown = true;
        for(thread& t : this->threads)
        { 
            t.join();
        }
    }
private:
    bool shuttingDown;
};
