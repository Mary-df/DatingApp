import { Photo } from "./photo";

export interface Member {
    id: number;
    userName: string;
    photoUrl: string;
    age: number;
    knownAs?: any;
    city: string;
    created: string;
    lastActive: string;
    gender: string;
    introduction: string;
    lookingFor: string;
    interests: string;
    country: string;
    photos: Photo[];
}


