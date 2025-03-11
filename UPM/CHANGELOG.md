# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.1] - 2025-03-11

### Added

- Added extensibility support through custom instance providers via `IDiBinding.ToInstanceProvider(IInstanceProvider provider)`

## [1.0.0] - 2025-03-10

### Added

- Basic implementation of DI container
- Support for dependency registration and resolution
- Automatic constructor dependency injection
- Support for singleton, transient and scoped dependencies
