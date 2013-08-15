James.Testing
=============

James.Testing is a library of test utilities.  It is named after the author who wrote the book of James in the Bible.  

"Dear brothers and sisters, when troubles come your way, consider it an opportunity for great joy. For you know that when your faith is tested, your endurance has a chance to grow."
(James 1:2-3)

Below is a description of the supported features.

Action Extensions
-----------------
1.  Executing an Action with Retries

Many times in integration tests, there is a non-deterministic time period between executing some initial action and asserting your expectations for the outcome.  In this case, it would be nice to have a method for automatically having the test retry your action for a number of times even though the assertion fails.  This method also supports setting a wait time in between retries so that you don't overload your system.

Example:

var counter = 0;
Action action = () => counter++;
action.ExecuteWithRetries(times, waitTimeInSeconds);


2.  Executing an Action with a Timeout Period

In other cases, you might want to execute a given action for a given time period.  For instance, if you have a requirement to expect a response within 30 seconds, you can set the maximum timeout period to 30 seconds for your assertions.  This method also allows a user to set a given wait time between executions of a given action.

Example:

var counter = 0;
Action action = () => counter++;
action.ExecuteWithTimeout(timeoutInSeconds, waitTimeInSeconds);