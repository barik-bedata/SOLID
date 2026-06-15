# Dependency Inversion Principle (DIP)

1. High-level modules should not depend on low-level modules. Both should depend on abstractions.
2. Abstractions should not depend on details. Details should depend on abstractions.

## Key Concept
- **Abstractions over Concretions**: Program to an interface, not an implementation.
- **Inversion of Control**: Control of dependency resolution is shifted from the calling class to a container or factory (often implemented via Dependency Injection).
