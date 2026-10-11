# Plisky.FlimFlam Constitution

## Core Principles

### I. Minimal, Standards-Conformant Implementation
If this file exists `X:\Code\ghub\ai-toolbox\agents\codeapproach.md` then use that as your coding approach.

If this file exists `X:\Code\ghub\ai-toolbox\agents\dotnet.md` then use that for dotnet coding standards.

If the files do not exist use the following information to guide your coding approach:

Before adding code, contributors MUST check whether the requirement is necessary, whether the
codebase already provides a suitable approach, whether the framework or platform provides a
solution, and whether an installed dependency applies, in that order. Contributors MUST write the
smallest implementation that meets the requirement, avoid unrequested abstractions and boilerplate,
and ask for approval before adding a dependency outside the standard framework or documented
preferred dependencies. Changes MUST account for input validation, error handling, logging, and
security.

For .NET code, contributors MUST follow the project's existing style and target framework; new
projects MUST use .NET 10 or later, while existing projects MUST NOT be upgraded unless the
approved plan explicitly requires it. C# code MUST use file-scoped namespaces, descriptive names,
explicit types for built-in types and `var` for other types, and One True Brace Style. Service
dependencies MUST use dependency injection. Comments MUST explain non-obvious reasoning only, and
nullable reference properties MUST NOT be assigned `null!` unless an external framework guarantees
their initialization. Code MUST meet IDE1006 naming rules, and new code MUST not increase warning
counts over baseline.

.NET unit tests MUST use xUnit and Shouldly, name the subject `sut`, follow
`Subject_condition_outcome` naming, and use Arrange / Act / Assert structure. Unit tests MUST NOT
access disk, databases, or networks. These constraints keep changes focused, consistent, and
compatible with the repository.

### II. Behavior-Driven Verification
Changes to externally observable behavior MUST include automated tests for the affected behavior.
Parsing, import, and other multi-component flows MUST be covered by integration tests when unit
tests alone cannot verify their contracts. Tests MUST be run for the affected projects before a
change is proposed for review.

### III. Windows Desktop Compatibility
The desktop application MUST remain compatible with running on Windows. Changes to the operating system, runtime, or distribution requirements MUST identify their compatibility impact and verification plan before adoption. This keeps the
viewer usable in the environment for which it is built.

### IV. Observable Diagnostics
Failures in trace loading, parsing, conversion, and external communication MUST be surfaced with
enough context for a user or maintainer to identify the problem. Code MUST NOT hide such failures
with silent catches or success-shaped fallbacks. Diagnostic output MUST avoid altering the trace
data being inspected.

### V. Focused, Maintainable Changes
Changes MUST address a defined user or maintenance need and MUST reuse existing project patterns
and dependencies when suitable. New dependencies, abstractions, or broader refactors MUST be
justified by a concrete requirement. This keeps the trace viewer maintainable without adding
unneeded complexity.

## Additional Constraints

The primary desktop application and UI controls target Windows. The repository also contains supporting projects with separate target frameworks; changes MUST preserve each affected project's declared target unless a reviewed change explicitly updates it.
Dependency or target-framework changes MUST be limited to what the requirement needs and verified by building the affected projects.



**Version**: 2.0.0 | **Ratified**: 11/10/2026 |
**Last Amended**: 2026-10-11
