export class VoteModel{
    questionId: number;
    newAnswer: string;
    oldAnswer: string;
    type: boolean;
}

export class UserVote{
    id: number;
    voteDate: Date;
    questionId: number;
    questionTitle: string;
    answer: string;
}

export class Vote{
    id: number;
    voted: boolean;
    voteDate: Date;
    questionId: number;
    questionTitle: string;
}