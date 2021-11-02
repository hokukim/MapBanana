import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Store } from '@ngrx/store';
import { IAppState } from '../app.state';
import { BananaApiService } from '../services/banana-api.service';
import { CampaignHubService } from '../services/campaign-hub.service';
import { selectCampaign } from '../store/campaign/campaign.selector';
import { selectMaps } from '../store/maps/maps.selector';

@Component({
  selector: 'app-campaign',
  templateUrl: './campaign.component.html',
  styleUrls: ['./campaign.component.scss']
})
export class CampaignComponent implements OnInit {

  public maps$ = this.store.select(selectMaps);
  public campaign$ = this.store.select(selectCampaign);
  private smallFile!: File;
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
      this.bananaApiService.getCampaign(this.campaignId);
    });
  }

  onMapThumbnailFile(event: any) {
    this.smallFile = event.target.files[0];
  }
  onMapFile(event: any) {
    this.file = event.target.files[0];
  }

  onAddMap() {
    this.bananaApiService.addMap(this.campaignId, this.file, this.smallFile);
  }

  onActivateMap( mapId: string) {
    this.bananaApiService.activateMap(this.campaignId, mapId);
  }
}
