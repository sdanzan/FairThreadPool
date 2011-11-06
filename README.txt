A simple thread pool allowing scheduling of jobs in a "fair" way. Jobs are
enqueued in the pool associated to a tag. To pick a job to run, the threads
in the pool cycle through the tags in round robin. Jobs with the same tag 
are picked in fifo order.

If you compile the project through the FairThreadPool.sln solution, a test
dll requiring NUunit is also built. NUnit dlls are xpexted to be found in
the 'externals' folder. To compile without the tests you may use the
FairThreadPoolWithoutTest.sln solution.
