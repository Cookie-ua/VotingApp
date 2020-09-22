export class Question {
    id: number;
    title: string;
    description: string;
    publishDate: Date;
    expiryDate: Date;
    isActive: boolean;
    totalCount: number;
    answers: Answer[];
}

export class Answer {
    answer: string;
    count: number;
    voted: boolean;
}