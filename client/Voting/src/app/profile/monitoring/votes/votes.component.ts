import { Component, OnInit } from '@angular/core';
import { UserVote, Vote } from 'src/app/core/models/Vote';
import { ProfileService } from '../../profile.service';

@Component({
  selector: 'app-votes',
  templateUrl: './votes.component.html',
  styleUrls: ['./votes.component.css']
})
export class ProfileVotesComponent implements OnInit {

  public votes: UserVote[];

  constructor(private profileService: ProfileService) { }

  ngOnInit() {
    this.getAllVotes();
  }

  private getAllVotes(){
    this.profileService.getAllVotes().subscribe((res: UserVote[]) => {
      this.votes = res;
    });
  }
}
