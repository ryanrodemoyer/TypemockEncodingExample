# Unfakeable Types and Updating Code

## Agenda

1. **mscorlib**  
Fakeable types via Typemock (below) or experience a TypemockException.
1. **Gist.**  
http://bit.ly/2ieJbl1
1. Scenario.

### Scenario
We need to make a small update within a much larger set of functionality:
1. **How do we test that change?**  
Test the smallest slice of functionality as possible.
1. **How do we make that change?**  
Write a test that verifies the behavior or confirms the bug, then make updates.

 ## Typemock and mscorlib
http://www.typemock.com/mscorlib-types 

Typemock Isolator supports the following types within mscorlib:

* System
	* DateTime
	* Environment
* System.IO
	* File
	* Directory
	* FileStream
* System.Security.Cryptography.X509Certificates
	* X509Certificate

## Exception with unfakeable types
TypeMock.TypeMockException : 
*** Exception throw while recording calls. Please check:
 * Are you trying to fake a field instead of a property? try to set field or use Isolate.Invoke.StaticConstructor
 * Are you are trying to fake an unsupported mscorlib method? See supported types here: http://www.typemock.com/mscorlib-types 
InnerException
  ----> System.Reflection.TargetInvocationException : Exception has been thrown by the target of an invocation.
  ----> System.ArgumentNullException : String reference not set to an instance of a String.
Parameter name: s