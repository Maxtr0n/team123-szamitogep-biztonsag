export class GetGifResponse {
    id: number;
    username: string;
    description: string;
    base64Encode: string;
    metadata: string;
}

export class CaffMetadata {
    creation_date: CaffCreationDate;
    creator: string;
    frames: CaffFrame[];
    height: number;
    width: number;
}

export class CaffCreationDate {
    day: number;
    hour: number;
    minute: number;
    month: number;
    year: number;
}

export class CaffFrame {
    caption: string;
    counter: number;
    tags: string[];
}