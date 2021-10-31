import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { IAppState } from '../app.state';
import { BananaHubService } from '../services/banana-api.service';

@Component({
  selector: 'app-campaign',
  templateUrl: './campaign.component.html',
  styleUrls: ['./campaign.component.scss']
})
export class CampaignComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private store: Store<IAppState>,
    private bananaHubService: BananaHubService) {
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const campaignId = params['id'];
      this.bananaHubService.getMaps(campaignId);
    });
  }
}
