// user.model.ts
export interface AuthResponse {
    token : string;
    name : string;
    email : string;
    role :string;
}
export interface LoginRequest { email: string; password: string; }
export interface RegisterRequest { name: string; email: string; password: string; }

// hotel.model.ts   
export interface Hotel{
    id:number;
    name:string;
    city : string;
    address : string;
    description : string;
    rating : number;
    imageUrl : string;
}

export interface Room{
    id:number;
    hotelId:number;
    hotelName:string;
    roomType:string;
    pricePerNight:number;
    capacity:number;
    isAvailable:boolean

}

// booking.model.ts
export interface Booking {
  id: number;
  userId: number;
  userName: string;
  roomId: number;
  roomType: string;
  hotelName: string;
  checkInDate: string;
  checkOutDate: string;
  totalPrice: number;
  status: string;
  bookingRef: string;
  createdAt: string;
}
export interface CreateBooking {
  roomId: number;
  roomType: string;
  hotelName: string;
  checkInDate: string;
  checkOutDate: string;
  pricePerNight: number;
}

