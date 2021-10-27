import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CampaignHubService } from '../services/campaign-hub.service';

@Component({
  selector: 'app-player',
  templateUrl: './player.component.html',
  styleUrls: ['./player.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PlayerComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private campaignHubService: CampaignHubService) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const campaignId: string = params['campaignId'];

      if (campaignId === undefined)
      {
        return;
      }

      this.campaignHubService.connect(campaignId);
    });
  }
}
