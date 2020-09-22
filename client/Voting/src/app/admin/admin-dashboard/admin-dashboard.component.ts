import { Component, OnInit } from '@angular/core';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {

  dashboardInfo: any[];
  totalQuestionCount: number;

  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.getDashboardInfo();
  }

  private getDashboardInfo(){
    this.adminService.getDashboardInfo().subscribe((res: any[]) => {
      this.totalQuestionCount = res.map(x => x.count).reduce((a, b) => a + b);
      this.dashboardInfo = res
    });
  }
}
