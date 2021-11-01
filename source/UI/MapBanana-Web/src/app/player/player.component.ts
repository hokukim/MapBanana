import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { IAppState } from '../app.state';
import { BananaApiService } from '../services/banana-api.service';
import { CampaignHubService } from '../services/campaign-hub.service';
import { selectActiveMapId, selectCampaign } from '../store/campaign/campaign.selector';
import { IMap } from '../store/maps/maps.model';
import { selectMap } from '../store/map/map.selector';

@Component({
  selector: 'app-player',
  templateUrl: './player.component.html',
  styleUrls: ['./player.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PlayerComponent implements OnInit {

  public activeMap$ = this.store.select(selectMap);
  private activeMapId$ = this.store.select(selectActiveMapId);
  private campaignId!: string;

  constructor(
    private route: ActivatedRoute,
    private store: Store<IAppState>,
    private bananaApiService: BananaApiService,
    private campaignHubService: CampaignHubService) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.campaignId = params['campaignId'];
      this.campaignHubService.connect(this.campaignId);
      this.bananaApiService.getCampaign(this.campaignId);
    });

    this.activeMapId$.subscribe(id => {
      this.bananaApiService.getMap(id);
    });
  }
}
