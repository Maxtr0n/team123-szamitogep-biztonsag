import { GetCommentResponse } from "./comment/GetCommentResponse";

export class CommentsDialogData {
    caffId: number;
    comments: GetCommentResponse[];
    newComment: string;
}