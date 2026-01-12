namespace Net9Odev.DTOs;

public record LabelResponseDto(
    int Id,
    string Name,
    string Country,
    DateTime CreatedAt
);

public record CreateLabelDto(
    string Name,
    string Country
);

public record UpdateLabelDto(
    string Name,
    string Country
);