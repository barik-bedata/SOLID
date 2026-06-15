# Open-Closed Principle (OCP)

Software entities (classes, modules, functions, etc.) should be open for extension, but closed for modification.

This means you should be able to extend a class's behavior without modifying its existing source code.

## Key Concept
- **Abstraction**: Use interfaces or abstract classes to define a contract.
- **Polymorphism**: Implement different behaviors by creating new classes that implement or extend the abstraction rather than modifying existing ones.
