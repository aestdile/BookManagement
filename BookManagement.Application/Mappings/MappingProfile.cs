using AutoMapper;
using BookManagement.Application.DTOs.Auth;
using BookManagement.Application.DTOs.Books;
using BookManagement.Application.DTOs.Borrowing;
using BookManagement.Domain.Entities;

namespace BookManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable()));

        CreateMap<CreateBookDto, Book>()
            .ForMember(dest => dest.AvailableCopies, opt => opt.MapFrom(src => src.TotalCopies));

        CreateMap<UpdateBookDto, Book>()
            .ForMember(dest => dest.ISBN, opt => opt.Ignore());

        CreateMap<BorrowRecord, BorrowRecordDto>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
            .ForMember(dest => dest.BookAuthor, opt => opt.MapFrom(src => src.Book.Author))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue()))
            .ForMember(dest => dest.OverdueDays, opt => opt.MapFrom(src => src.IsOverdue() ? src.GetOverdueDays() : (int?)null));

        CreateMap<BorrowRecord, BorrowHistoryDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
            .ForMember(dest => dest.UserPhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}