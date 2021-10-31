import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Store } from '@ngrx/store';
import { IAppState } from '../app.state';
import { BananaApiService } from '../services/banana-api.service';
import { CampaignHubService } from '../services/campaign-hub.service';
import { selectMaps } from '../store/maps/maps.selector';

@Component({
  selector: 'app-campaign',
  templateUrl: './campaign.component.html',
  styleUrls: ['./campaign.component.scss']
})
export class CampaignComponent implements OnInit {

  public maps$ = this.store.select(selectMaps);
  private file!: File;
  private campaignId!: string;

  constructor(
    private route: ActivatedRoute,
    private store: Store<IAppState>,
    private bananaApiService: BananaApiService,
    private campaignHubService: CampaignHubService) {
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.campaignId = params['id'];
      this.campaignHubService.connect(this.campaignId);
      this.bananaApiService.getMaps(this.campaignId);
    });
  }

  onMapFile(event: any) {
    this.file = event.target.files[0];
  }

  onAddMap() {
    this.bananaApiService.addMap(this.campaignId, this.file);
  }

  onActivateMap( mapId: string) {
  }
}
