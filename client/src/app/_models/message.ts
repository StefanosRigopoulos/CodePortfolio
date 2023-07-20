export interface Message {
    id: number;
    senderId: number;
    senderUsername: string;
    senderMainPhotoURL: string;
    recipientId: number;
    recipientUsername: string;
    recipientMainPhotoURL: string;
    content: string;
    dateRead?: Date;
    messageSent: Date;
}