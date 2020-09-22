export class Comment{
    id: number;
    message: string;
    publishDate: Date;
    isMine: boolean;
    isBlocked: boolean;
    commentId: number;
    userId: string;
    userName: string;
    questionId: number;

    answersCount: number;
    comments: Comment[];
}