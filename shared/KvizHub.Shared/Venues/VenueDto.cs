namespace KvizHub.Shared.Venues;

public record VenueDto(
    int Id, string Name, string Street, string PostalCode, string City, int DefaultCapacity
);