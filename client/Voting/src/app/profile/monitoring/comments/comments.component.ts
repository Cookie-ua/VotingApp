import { Component, OnInit } from '@angular/core';
import { Comment } from 'src/app/core/models/Comment';
import { ProfileService } from '../../profile.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css']
})
export class ProfileCommentsComponent implements OnInit {

  public comments: Comment[];
  constructor(private profileService: ProfileService) { }

  ngOnInit() {
    this.getAllMyComments();
  }

  private getAllMyComments(){
    this.profileService.getAllComments().subscribe((res: Comment[]) => {
      this.comments = res;
    });
  }
}
