# Temporal

An ASP.NET library enabling easy time traveling at runtime.

## Installation

TODO

<!--
The package is not yet ready:

Install the [Temporal](https://www.nuget.org/packages/Temporal) NuGet package.

```
PM> Install-Package Temporal
```
-->

## Usage

Wire up Temporal using `UseTemporal`.

Now, to enable individual time travel replace usages of `DateTime.Now` and `DateTime.UtcNow` with `TemporalClock.Now` and `TemporalClock.UtcNow`, respectively. **Any time freeze requested is scoped to that user only.** When a user freezes time, a session cookie is used to store where that user is in *time*.

**Note:** Requests, including client-side requests, should include the `CookieTimeProvider` cookie (`__TemporalTime`) to take advantage of time freezing. Otherwise, the current time may be used expectedly.

## Implementation notes

- `UseTemporal` wires up endpoints for the on-screen *TimeMachine* to use, which enables users to view, freeze at a specific date and time, and unfreeze time.
- When `UseTemporal` is called, the Temporal endpoints will be available. You may not want this behavior in production.
- A static class `TemporalTime` is used, rather than an `ITemporalClock` or equivalent interface. This makes it easier to use as necessary wherever you are in code, without having to worry about getting an instance of the `ITemporalClock` implementation from IoC, etc.

## Contributing

Have an idea? Let's talk about it in an issue!

Find a bug? Open an issue or submit a pull request!

## License

MIT License
