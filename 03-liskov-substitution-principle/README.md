# Liskov Substitution Principle (LSP)

Subtypes must be substitutable for their base types.

If $S$ is a subtype of $T$, then objects of type $T$ may be replaced with objects of type $S$ without altering any of the desirable properties of the program (correctness, task performed, etc.).

## Key Concept
- **Behavioral Subtyping**: A subclass should not only match the interface of the parent class but also its expected behavior/semantics.
- **Contracts**: Subclasses should not strengthen preconditions (inputs) or weaken postconditions (outputs).
