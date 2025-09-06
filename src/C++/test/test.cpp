#include <list>
#include <iostream>
#include <thread>

#include "test.h"

using namespace std;

int main()
{
    std::list<std::thread>* l = new std::list<std::thread> (2, 100);
    std::thread t = &( new std::thread (test) );
    l.push_front(t);
    //t->join();
    return 0;
}
void test()
{
    cout << "thread";
}
